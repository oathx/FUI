using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace FUISourcesGenerator
{
    internal class ViewModelModifier : ITypeSyntaxNodeModifier
    {
        public SyntaxNode Modify(SyntaxNode root)
        {
            return ReplaceViewModelToPublicAndPartial(root);
        }

        /// <summary>
        /// 将所有拥有Binding这个特性的类更改为public partial class
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        SyntaxNode ReplaceViewModelToPublicAndPartial(SyntaxNode root)
        {
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToArray();
            foreach (var classDeclaration in classDeclarations)
            {
                if (!IsBindable(classDeclaration))
                {
                    continue;
                }

                //如果是静态类或者抽象类直接不管
                if(classDeclaration.Modifiers.Any((k)=>k.IsKind(SyntaxKind.StaticKeyword) || k.IsKind(SyntaxKind.AbstractKeyword)))
                {
                    continue;
                }

                var newClass = classDeclaration.WithModifiers(SyntaxFactory.TokenList());
                newClass = newClass.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword)).NormalizeWhitespace();
                newClass = newClass.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword)).NormalizeWhitespace();
                root = root.ReplaceNode(classDeclaration, newClass);
                Console.WriteLine("======================================ReplaceViewModelToPublicAndPartial===================================");
                Console.WriteLine(newClass);
            }
            
            return root;
        }

        bool IsBindable(ClassDeclarationSyntax classDeclaration)
        {
            var classAttributes = classDeclaration.AttributeLists.ToList();
            
            foreach(var att in classAttributes)
            {
                foreach(var node in att.ChildNodes().OfType<AttributeSyntax>())
                {
                    foreach(var id in node.ChildNodes().OfType<IdentifierNameSyntax>())
                    {
                        if (id.ToString() == "Binding" 
                            || id.ToString() == "BindingAttribute" 
                            || id.ToString() == "FUI.Bindable.BindingAttribute" 
                            || id.ToString() == "Bindable.BindingAttribute" 
                            || id.ToString() == "FUI.Bindable.Binding" 
                            || id.ToString() == "Bindable.Binding")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
