using Mono.Cecil;
using Mono.Cecil.Cil;

using System.Reflection;

namespace FUISourcesGenerator
{
    public class PropertyChangedInjector : ITypeDefinationInjector
    {
        const string observableObjectTypeName = "FUI.Bindable.ObservableObject";
        const string propertyChangedMethodName = "OnPropertyChanged";
        const string bindingAttributeTypeName = "FUI.BindingAttribute";
        const string fuiPath = "../../../../../../Library/ScriptAssemblies/FUI.Runtime.dll";

        void ITypeDefinationInjector.Inject(Mono.Cecil.ModuleDefinition moduleDefinition, Mono.Cecil.TypeDefinition typeDefinition)
        {
            Console.WriteLine($"inject Type:{typeDefinition}");
            foreach (var property in typeDefinition.Properties)
            {
                Console.WriteLine($"property:{property}");
                var hasBindingAttribute = property.HasCustomAttribute(bindingAttributeTypeName);
                var hasSetMethod = property.SetMethod != null;
                if (!hasBindingAttribute || !hasSetMethod)
                {
                    continue;
                }

                Console.WriteLine($"Inject type:{typeDefinition} property:{property} propertyChanged");
                InjectPropertyChangedMethod(moduleDefinition, property, propertyChangedMethodName);
            }

            Console.WriteLine($"inject PropertyChanged complete");
        }

        /// <summary>
        /// 向可绑定的属性Set方法注入属性更改委托
        /// </summary>
        /// <param name="module"></param>
        /// <param name="property"></param>
        /// <param name="propertyChangedMethodName"></param>
        void InjectPropertyChangedMethod(ModuleDefinition module, PropertyDefinition property, string propertyChangedMethodName)
        {
            var flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            var type = Assembly.LoadFrom(fuiPath).GetType(observableObjectTypeName);
            var propertyChangedMethod = module.ImportReference(type.GetMethod(propertyChangedMethodName, flag));
            var injector = new Injector(property.SetMethod, 2);
            injector.InsertAfter(OpCodes.Ldarg_0);
            injector.InsertAfter(OpCodes.Ldstr, property.Name);
            injector.InsertAfter(OpCodes.Call, propertyChangedMethod);
            injector.OffsetMethod();
        }
    }
}
