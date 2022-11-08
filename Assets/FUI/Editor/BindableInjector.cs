using Mono.Cecil;
using Mono.Cecil.Cil;

using System;
using System.Collections.Generic;

namespace FUI.Editor
{
    public class FUIInjector
    {
        [UnityEditor.MenuItem("Tools/Test")]
        public static void Inject()
        {
            //TODO 只加载目标dll  依赖的dll通过module.refrenceassemblies 来加载
            const string dllPath = "./Library/ScriptAssemblies/FUI.Test.dll";
            //List<string> refrences = new List<string>
            //{
            //    "./Library/ScriptAssemblies/FUI.Runtime.dll"
            //};
            const string propertyChangedHandlerName = "PropertyChanged";
            //refrences.Add(dllPath);
            var modules = LoadModules(new List<string> { dllPath});
            InjectPropertyChanged(modules, dllPath, propertyChangedHandlerName);
        }

        static List<ModuleDefinition> LoadModules(List<string> dlls)
        {
            var modules = new List<ModuleDefinition>();
            foreach(var dll in dlls)
            {
                var module = ModuleDefinition.ReadModule(dll);
                modules.Add(module);
            }
            return modules;
        }

        static void InjectPropertyChanged(List<ModuleDefinition> modules, string dllPath, string propertyChangedHandlerName)
        {
            foreach(var type in modules[0].Types)
            {
                UnityEngine.Debug.Log(type);
                foreach (var property in type.Properties)
                {
                    if (!property.HasCustomAttribute<BindingAttribute>())
                    {
                        continue;
                    }

                    InjectPropertySetMethod(modules[0], type, property, propertyChangedHandlerName);
                }
            }
                //assembly.Write(dllPath, new WriterParameters { WriteSymbols = true });
        }

        static void InjectPropertySetMethod(ModuleDefinition module, TypeDefinition type, PropertyDefinition property, string propertyChangedHandlerName)
        {
            var actionMethod = module.ImportReference(typeof(Action<object, string>).GetMethod("Invoke", new[] { typeof(object), typeof(string) }));
            var injector = new Injector(property.SetMethod, 2);
            injector.InsertAfter(OpCodes.Ldarg_0);
            injector.InsertAfter(OpCodes.Call, type.GetProperty(propertyChangedHandlerName).GetMethod);
            injector.InsertAfter(OpCodes.Ldarg_0);
            injector.InsertAfter(OpCodes.Ldstr, property.Name);
            injector.InsertAfter(OpCodes.Callvirt, actionMethod);
            injector.OffsetMethod();
            UnityEngine.Debug.Log($"inject PropertyChanged complete");
        }
    }
}
