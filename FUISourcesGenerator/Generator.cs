using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

namespace FUISourcesGenerator
{
    

    internal class Generator
    {
        public string TestString = @"
public class BindingAttribute : System.Attribute{}

public class ObserverbleObject{}

public class ViewModel : ObserverbleObject{}

[Binding]
public class TestViewModel : ViewModel
{
    [Binding]
    public int a;
    [Binding]
    public int b;
    [Binding]
    public int c;
}
";
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
