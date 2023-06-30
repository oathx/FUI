using FUI.Bindable;

using Mono.Cecil;
using Mono.Cecil.Cil;

using System.Reflection;

namespace FUI.Editor
{
    public class FUIInjector
    {
        [UnityEditor.MenuItem("Tools/FUI/InjectPropertyChanged")]
        public static void Inject()
        {
            //TODO 只加载目标dll  依赖的dll通过module.refrenceassemblies 来加载
            const string dllPath = "./Library/ScriptAssemblies/FUI.Test.dll";
            const string propertyChangedMethodName = "OnPropertyChanged";
            InjectPropertyChanged(dllPath, propertyChangedMethodName);
        }

        /// <summary>
        /// 注入属性更改通知方法
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="propertyChangedMethodName"></param>
        static void InjectPropertyChanged(string dllPath, string propertyChangedMethodName)
        {
            var assembly = AssemblyDefinition.ReadAssembly(dllPath);
            foreach(var type in assembly.MainModule.Types)
            {
                foreach (var property in type.Properties)
                {
                    if (!property.HasCustomAttribute<BindingAttribute>() || property.SetMethod == null)
                    {
                        continue;
                    }

                    InjectPropertyChangedMethod(assembly.MainModule, property, propertyChangedMethodName);
                }
            }
            assembly.Write(dllPath, new WriterParameters { WriteSymbols = true });
            UnityEngine.Debug.Log($"inject PropertyChanged complete");
        }

        /// <summary>
        /// 向可绑定的属性Set方法注入属性更改委托
        /// </summary>
        /// <param name="module"></param>
        /// <param name="property"></param>
        /// <param name="propertyChangedMethodName"></param>
        static void InjectPropertyChangedMethod(ModuleDefinition module, PropertyDefinition property, string propertyChangedMethodName)
        {
            var flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            var propertyChangedMethod = module.ImportReference(typeof(ObservableObject).GetMethod(propertyChangedMethodName, flag));
            var injector = new Injector(property.SetMethod, 2);
            injector.InsertAfter(OpCodes.Ldarg_0);
            injector.InsertAfter(OpCodes.Ldstr, property.Name);
            injector.InsertAfter(OpCodes.Call, propertyChangedMethod);
            injector.OffsetMethod();
        }
    }
}
