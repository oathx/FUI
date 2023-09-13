using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FUISourcesGenerator
{
    public class PropertyChangedInjector : ITypeDefinationInjector
    {
        void ITypeDefinationInjector.Inject(Mono.Cecil.ModuleDefinition moduleDefinition, Mono.Cecil.TypeDefinition typeDefinition)
        {
            Console.WriteLine($"inject Type:{typeDefinition}");
            foreach (var property in typeDefinition.Properties)
            {
                Console.WriteLine($"property:{property}");
                var hasBindingAttribute = property.HasCustomAttribute("FUI.ObservablePropertyAttribute");
                var hasSetMethod = property.SetMethod != null;
                if (!hasBindingAttribute || !hasSetMethod)
                {
                    continue;
                }

                Console.WriteLine($"Inject type:{typeDefinition} property:{property} propertyChanged");
                InjectPropertyChangedMethod(moduleDefinition, typeDefinition, property);
            }

            Console.WriteLine($"inject PropertyChanged complete");
        }

        /// <summary>
        /// 向可绑定的属性Set方法注入属性更改委托
        /// </summary>
        /// <param name="module"></param>
        /// <param name="property"></param>
        /// <param name="propertyChangedMethodName"></param>
        void InjectPropertyChangedMethod(ModuleDefinition module, TypeDefinition type, PropertyDefinition property)
        {
            property.SetMethod.Body.Instructions.Clear();
            var processor = property.SetMethod.Body.GetILProcessor();

            var ret = processor.Create(OpCodes.Ret);
            var callChangedNop = processor.Create(OpCodes.Nop);
            var changedIsNullNop = processor.Create(OpCodes.Nop);   

            // _Name_Changed?.Invoke(this, _name, value);
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            var propertyChangedField = type.GetField($"_{property.Name}_Changed");
            processor.Append(processor.Create(OpCodes.Ldfld, propertyChangedField));
            processor.Append(processor.Create(OpCodes.Dup));
            processor.Append(processor.Create(OpCodes.Brtrue_S, callChangedNop));
            processor.Append(processor.Create(OpCodes.Pop));
            processor.Append(processor.Create(OpCodes.Br_S, changedIsNullNop));

            processor.Append(callChangedNop);
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            var field = module.ImportReference(type.GetField($"<{property.Name}>k__BackingField"));
            processor.Append(processor.Create(OpCodes.Ldfld, field));
            processor.Append(processor.Create(OpCodes.Ldarg_1));
            processor.Append(processor.Create(OpCodes.Callvirt, propertyChangedField.FieldType.Resolve().GetMethod("Invoke")));

            // _name = value;
            processor.Append(changedIsNullNop);
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldarg_1));
            processor.Append(processor.Create(OpCodes.Stfld, field));
            processor.Append(ret);
        }
    }
}
