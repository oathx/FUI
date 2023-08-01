using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

using Mono.Cecil;

namespace FUISourcesGenerator
{
    internal class Generator
    {
        public List<ITypeSyntaxNodeSourcesGenerator> typeSyntaxRootGenerators = new List<ITypeSyntaxNodeSourcesGenerator>();
        public List<ITypeDefinationSourcesGenerator> typeDefinationSourcesGenerators = new List<ITypeDefinationSourcesGenerator>();
        public List<IBeforeCompilerSourcesGenerator> beforeCompilerSourcesGenerators = new List<IBeforeCompilerSourcesGenerator>();
        public List<ITypeDefinationInjector> typeDefinationInjectors = new List<ITypeDefinationInjector>();

        public async Task LoadProject(string solutionPath, string projectName, string output)
        {
            MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();
            Console.WriteLine("Loading solution '{0}'", solutionPath);
            var solution = await workspace.OpenSolutionAsync(solutionPath);
            Console.WriteLine(solution.ToString());
            var project = solution.Projects.FirstOrDefault(item => item.Name == projectName);

            List<Source> addition = new List<Source>();
            void AddSources(Source?[] sources)
            {
                if(sources == null)
                {
                    return;
                }

                foreach(var source in sources)
                {
                    if(source != null)
                    {
                        addition.Add(source.Value);
                    }
                }
            }
            foreach(var document in project.Documents)
            {
                Console.WriteLine(document.Name);
                var root = await document.GetSyntaxRootAsync();
                if(root == null)
                {
                    continue;
                }

                //根据类型语法树生成代码
                typeSyntaxRootGenerators.ForEach((item)=>AddSources(item.Generate(root)));
            }

            //根据类型定义生成代码
            var assembly = await Compiler(project);
            foreach(var module in assembly.Modules)
            {
                foreach(var type in module.Types)
                {
                    typeDefinationSourcesGenerators.ForEach((item) => AddSources(item.GetSource(module, type)));
                }
            }

            //编译前源代码生成器
            beforeCompilerSourcesGenerators.ForEach(item => AddSources(item.Generate()));

            //注入代码
            foreach (var add in addition)
            {
                project.AddAdditionalDocument(add.name, add.Text);
            }

            var result = await Compiler(project);
            //注入IL
            foreach(var injector in typeDefinationInjectors)
            {
                foreach(var module in result.Modules)
                {
                    foreach(var type in module.Types)
                    {
                        injector.Inject(module, type);
                    }
                }
            }

            //输出文件
            result.Write(output);
        }

        async Task<AssemblyDefinition> Compiler(Project project)
        {
            var compilation = await project.GetCompilationAsync();
            var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }

            ms.Seek(0, SeekOrigin.Begin);
            return AssemblyDefinition.ReadAssembly(ms);
        }

        public async void Generate(string output, string solutionPath, string projectName)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(solutionPath);
            var project = solution.Projects.FirstOrDefault(item => item.Name == projectName);
            foreach(var document in project.Documents)
            {
                var root = await document.GetSyntaxRootAsync();
                ReplaceViewModelToPublicAndPartial(root);
            }
        }

        void ReplaceViewModelToPublicAndPartial(SyntaxNode root)
        {
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToArray();
            foreach(var classDeclaration in classDeclarations)
            {
                if(!HasBaseClass(classDeclaration, "ViewModel"))
                {
                    continue;
                }

                var partialModifier = SyntaxFactory.Token(SyntaxKind.PartialKeyword).WithAdditionalAnnotations(Formatter.Annotation);
                var newNode = classDeclaration.AddModifiers(partialModifier);
                root.ReplaceNode(classDeclaration, newNode);
            }
        }

        bool HasBaseClass(ClassDeclarationSyntax classDeclatation, string baseClassName)
        {
            if(classDeclatation.BaseList == null)
            {
                return false;
            }

            return classDeclatation.BaseList.Types.FirstOrDefault(item => item.ToString() == baseClassName) != null;
        }

        void ReplaceFieldToBindableProperty(SyntaxNode root)
        {
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToArray();
            foreach(var classDeclaration in classDeclarations)
            {
                foreach(var member in classDeclaration.Members)
                {
                    if(member.AttributeLists.FirstOrDefault(item=>item.ToString() == "Binding") == null)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
