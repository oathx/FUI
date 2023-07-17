using Mono.Cecil;
using Mono.Cecil.Cil;

using System.Reflection;

namespace FUISourcesGenerator
{
    public class FUIInjector
    {
        const string propertyChangedMethodName = "OnPropertyChanged";
        const string output = "../output/test.dll";

        public static void Inject(MemoryStream assemblyStream)
        {
            InjectPropertyChanged(assemblyStream);
        }

        /// <summary>
        /// 注入属性更改通知方法
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="propertyChangedMethodName"></param>
        static void InjectPropertyChanged(MemoryStream assemblyStream)
        {
            var assembly = AssemblyDefinition.ReadAssembly(assemblyStream);
            foreach (var type in assembly.MainModule.Types)
            {
                Console.WriteLine(type?.Name);
                foreach (var property in type.Properties)
                {
                    if (!property.HasCustomAttribute("BindingAttribute") || property.SetMethod == null)
                    {
                        continue;
                    }

                    InjectPropertyChangedMethod(assembly.MainModule, property, propertyChangedMethodName);
                }
            }

            assembly.Write(output);//, new WriterParameters { WriteSymbols = true });
            Console.WriteLine($"inject PropertyChanged complete");
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
            var type = Assembly.LoadFrom("").GetType("ObservableObject");
            var propertyChangedMethod = module.ImportReference(type.GetMethod(propertyChangedMethodName, flag));
            var injector = new Injector(property.SetMethod, 2);
            injector.InsertAfter(OpCodes.Ldarg_0);
            injector.InsertAfter(OpCodes.Ldstr, property.Name);
            injector.InsertAfter(OpCodes.Call, propertyChangedMethod);
            injector.OffsetMethod();
        }
    }
}
