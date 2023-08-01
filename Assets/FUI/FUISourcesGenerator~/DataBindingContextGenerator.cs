using Newtonsoft.Json;

using System.Text;

namespace FUISourcesGenerator
{
    public class BindingProperty
    {
        public string name;
        public string type;
        public string elementPath;
        public string elementType;
    }

    public class BindingContext
    {
        public string type;
        public List<BindingProperty> properties;
    }

    public class BindingConfig
    {
        public string viewName;
        public List<BindingContext> bindingContexts;
        public BindingContext defaultContext;
    }
    public partial class DataBindingContextGenerator : IBeforeCompilerSourcesGenerator
    {
        const string configPath = "./UIBindingConfig";
        const string configExtension = ".binding";

        Source?[] IBeforeCompilerSourcesGenerator.Generate()
        {
            var result = new List<Source?>();
            foreach (var file in Directory.GetFiles(configPath, $"*{configExtension}"))
            {
                var config = JsonConvert.DeserializeObject<BindingConfig>(File.ReadAllText(file));
                var code = Template.Replace(viewNameMark, config.viewName);

                HashSet<string> propertyChangedNames = new HashSet<string>();
                //生成一个context的属性变化委托
                var onPropertyChangedBuilder = new StringBuilder();
                foreach (var bindingContext in config.bindingContexts)
                {
                    var contextPropertyChanged = OnPropertyChangedTemplate.Replace(viewModelTypeMark, GetFormattedType(bindingContext.type));
                    contextPropertyChanged = contextPropertyChanged.Replace(viewModelNameMark, GetTypeShortName(bindingContext.type));
                    var contextBuilder = new StringBuilder();
                    foreach (var property in bindingContext.properties)
                    {
                        var delegateName = $"{property.name}_{GetFormattedType(property.type)}_Delegate";
                        propertyChangedNames.Add(delegateName);
                        contextBuilder.AppendLine($"case @\"{property.name}\": {delegateName}?.Invoke({GetTypeShortName(bindingContext.type)}.{property.name}) break;");
                    }
                    contextPropertyChanged = contextPropertyChanged.Replace(propertyChangedCasesMark, contextBuilder.ToString());
                    onPropertyChangedBuilder.Append(contextPropertyChanged);
                }

                //生成默认context的属性变化委托
                var defaultPropertyChanged = OnPropertyChangedTemplate.Replace(viewModelTypeMark, "FUI.ViewModel");
                defaultPropertyChanged = defaultPropertyChanged.Replace(viewModelNameMark, "context");
                var defaultContextBuilder = new StringBuilder();
                foreach(var property in config.defaultContext.properties)
                {
                    var delegateName = $"{property.name}_{GetFormattedType(property.type)}_Delegate";
                    propertyChangedNames.Add(delegateName);
                    defaultContextBuilder.AppendLine($"case @\"{property.name}\": {delegateName}?.Invoke(context.GetProperty<{property.type}>(@\"{property.name}\")) break;");
                }

                onPropertyChangedBuilder.Append(defaultPropertyChanged);

                code.Replace(onPropertyChangedMark, onPropertyChangedBuilder.ToString());
                Console.WriteLine(code);
                //result.Add(new Source($"{config.viewName}.DataBinding", code));
            }

            return result.ToArray();
        }

        static string GetFormattedType(string type)
        {
            return type.Replace(".", "_");
        }

        static string GetTypeShortName(string type)
        {
            return type.Split('.').Last();
        }
    }
}
