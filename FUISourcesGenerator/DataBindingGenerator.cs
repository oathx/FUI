using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FUISourcesGenerator
{
    internal class DataBindingGenerator : ISourcesGenerator
    {
        Source? ISourcesGenerator.Generate(SyntaxNode root)
        {
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach(var classDeclaration in classDeclarations)
            {
                var classAttributes = classDeclaration.AttributeLists.ToList();
                if(!classAttributes.Exists((item)=>item.ToString() == "Binding"))
                {
                    continue;
                }

                var members = classDeclaration.Members;
                foreach(var member in members)
                {
                    var propertyDeclaration = member as PropertyDeclarationSyntax;
                    if(propertyDeclaration == null)
                    {
                        continue;
                    }
                    var attributeLists = propertyDeclaration.AttributeLists;
                    foreach(var attributeList in attributeLists)
                    {
                        var attributes = attributeList.Attributes;
                        foreach(var attribute in attributes)
                        {
                            var name = attribute.Name.ToString();
                            if(name == "Binding")
                            {
                                var text = propertyDeclaration.GetText().ToString();
                                Console.WriteLine(text);
                                //return new Source(propertyDeclaration.Identifier.ToString() + ".cs", text);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
