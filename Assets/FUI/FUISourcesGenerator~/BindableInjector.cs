using Mono.Cecil;
using Mono.Cecil.Cil;

using System.Reflection;
using System.Runtime.Intrinsics.Arm;

using static System.Net.Mime.MediaTypeNames;

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
        //before
            //// <ID>k__BackingField = value;
            //IL_0000: ldarg.0
            //IL_0001: ldarg.1
            //IL_0002: stfld int32 FUI.Test.TestViewModel::'<ID>k__BackingField'
            //// }
            //IL_0007: ret

        //after
            //// {
            //IL_0000: nop
            //// if (!EqualityComparer<Name>.Default.Equals(_name, value))
            //IL_0001: call class [netstandard] System.Collections.Generic.EqualityComparer`1<!0> class [netstandard] System.Collections.Generic.EqualityComparer`1<class FUI.Test.Name>::get_Default()
            //IL_0006: ldarg.0
            //IL_0007: ldfld class FUI.Test.Name FUI.Test.TestViewModel::_name
            //IL_000c: ldarg.1
            //IL_000d: callvirt instance bool class [netstandard] System.Collections.Generic.EqualityComparer`1<class FUI.Test.Name>::Equals(!0, !0)
            //IL_0012: stloc.0
            //// (no C# code)
            //IL_0013: ldloc.0
            //IL_0014: brfalse.s IL_0019
            //// }
            //IL_0016: nop
            //IL_0017: br.s IL_003a
            //// _Name_Changed?.Invoke(this, _name, value);
            //IL_0019: ldarg.0
            //IL_001a: ldfld class [FUI.Runtime] FUI.Bindable.PropertyChangedHandler`1<class FUI.Test.Name> FUI.Test.TestViewModel::_Name_Changed
            //IL_001f: dup
            //IL_0020: brtrue.s IL_0025
            //// (no C# code)
            //IL_0022: pop
            //IL_0023: br.s IL_0033
            //IL_0025: ldarg.0
            //IL_0026: ldarg.0
            //IL_0027: ldfld class FUI.Test.Name FUI.Test.TestViewModel::_name
            //IL_002c: ldarg.1
            //IL_002d: callvirt instance void class [FUI.Runtime] FUI.Bindable.PropertyChangedHandler`1<class FUI.Test.Name>::Invoke(object, !0, !0)
            //// _name = value;
            //IL_0032: nop
            //IL_0033: ldarg.0
            //IL_0034: ldarg.1
            //IL_0035: stfld class FUI.Test.Name FUI.Test.TestViewModel::_name
            //// (no C# code)
            //IL_003a: ret
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
