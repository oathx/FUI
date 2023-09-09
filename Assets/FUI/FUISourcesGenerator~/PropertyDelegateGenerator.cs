using Mono.Cecil;

using System.Text;

namespace FUISourcesGenerator
{
    internal class PropertyDelegateGenerator : ITypeDefinationSourcesGenerator
    {
        public Source?[] GetSource(ModuleDefinition moduleDefinition, TypeDefinition typeDefinition)
        {
            if (!typeDefinition.HasCustomAttribute("FUI.BindingAttribute"))
            {
                return null;
            }
            var properties = typeDefinition.Properties;
            if(properties.Count == 0)
            {
                return null;
            }

            var propertyDelegateBuilder = new StringBuilder();
            var hasNamespace = !string.IsNullOrEmpty(typeDefinition.Namespace);
            if (hasNamespace)
            {
                propertyDelegateBuilder.AppendLine($"namespace {typeDefinition.Namespace}");
                propertyDelegateBuilder.AppendLine("{");
            }
            
            propertyDelegateBuilder.AppendLine($"public partial class {typeDefinition.Name}");
            propertyDelegateBuilder.AppendLine("{");

            var count = 0;
            foreach(var property in properties)
            {
                var isBindingProperty = property.HasCustomAttribute("FUI.BindingAttribute");
                if (!isBindingProperty)
                {
                    continue;
                }

                count++;
                var propertyName = Utility.GetGenericTypeName(property.PropertyType.FullName);
                var propertyDelegateName = Utility.GetPropertyChangedDelegateName(property.Name);
                propertyDelegateBuilder.AppendLine($"public FUI.Bindable.PropertyChangedHandler<{propertyName}> {propertyDelegateName};");
            }
            if(count == 0)
            {
                return null;
            }
            propertyDelegateBuilder.AppendLine("}");
            if (hasNamespace)
            {
                propertyDelegateBuilder.AppendLine("}");
            }
            var code = Utility.NormalizeCode(propertyDelegateBuilder.ToString());
            Console.WriteLine(code);
            return new Source?[] {new Source($"{typeDefinition.FullName}.PropertyChanged", code)};
        }
    }
}
