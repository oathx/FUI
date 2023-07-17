using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FUISourcesGenerator
{
    internal struct Source
    {
        public readonly string name;
        public SourceText Text { get; private set; }

        public Source(string name, string text)
        {
            this.name = name;
            this.Text = SourceText.From(text);
        }

        public void BuildText(string text)
        {
            this.Text = SourceText.From(text);
        }
    }
    internal interface ISourcesGenerator
    {
        Source? Generate(SyntaxNode root);
    }
}
