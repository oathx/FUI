using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FUISourcesGenerator
{
    internal class Generator
    {
        public List<ISourcesGenerator> generators = new List<ISourcesGenerator>();

        public async Task LoadProject(string solutionPath, string projectName)
        {
            MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();
            UnityEngine.Debug.Log($"Loading solution {solutionPath}");
            var solution = await workspace.OpenSolutionAsync(solutionPath);
            UnityEngine.Debug.Log(solution.ToString());
            var project = solution.Projects.FirstOrDefault(item => item.Name == projectName);

            List<Source> addition = new List<Source>();
            foreach(var document in project.Documents)
            {
                UnityEngine.Debug.Log(document.Name);
                var root = await document.GetSyntaxRootAsync();
                if(root == null)
                {
                    continue;
                }

                foreach(var generator in generators)
                {
                    var source = generator.Generate(root);
                    if(source != null)
                    {
                        addition.Add(source.Value);
                    }
                }
            }

            foreach(var add in addition)
            {
                project.AddAdditionalDocument(add.name, add.Text);
            }

            var compilation = await project.GetCompilationAsync();
            Assembly assembly = null;
            using(var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if(!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        UnityEngine.Debug.Log($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                    }
                    return;
                }

                ms.Seek(0, SeekOrigin.Begin);
                assembly = Assembly.Load(ms.ToArray());
            }


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

                //var partialModifier = SyntaxFactory.Token(SyntaxKind.PartialKeyword).WithAdditionalAnnotations(Formatter.Annotation);
                //var newNode = classDeclaration.AddModifiers(partialModifier);
                //root.ReplaceNode(classDeclaration, newNode);
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
                    //if(member.AttributeLists.FirstOrDefault(item=>item.ToString() == "Binding") == null)
                    //{
                    //    continue;
                    //}
                }
            }
        }
    }
}
