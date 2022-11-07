using Mono.Cecil;
using Mono.Cecil.Cil;

using System;

using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace FUI.Editor
{
    public class Test
    {
        [UnityEditor.MenuItem("Tools/Test")]
        public static void Inject()
        {
            var path = "./Library/ScriptAssemblies/Assembly-CSharp.dll";
            Injector.Inject(path);
        }
    }
    internal class Worker
    {
        internal Instruction insertPoint { get; private set; }
        ILProcessor processor;
        MethodBody methodBody;
        string key;

        internal Worker(MethodDefinition methodDefinition, string key)
        {
            this.methodBody = methodDefinition.Body;
            var processor = methodDefinition.Body.GetILProcessor();
            var insertPoint = methodDefinition.Body.Instructions[0];
            this.insertPoint = insertPoint;
            this.processor = processor;
            this.key = key;
        }

        internal bool HasMark()
        {
            if (methodBody.Instructions.Count < 2)
            {
                return false;
            }

            var checkPoint = methodBody.Instructions[1].Operand;
            if (checkPoint != null && checkPoint.ToString() == key)
            {
                return true;
            }

            return false;
        }

        internal void OffsetMethod()
        {
            var offset = 0;
            foreach (var instruction in methodBody.Instructions)
            {
                instruction.Offset = offset;
                offset += instruction.GetSize();
            }
        }

        internal void InsertBefore(OpCode opCodes)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCodes));
        }

        internal void InsertBefore(OpCode opCode, string value)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, value));
        }

        internal void InsertBefore(OpCode opCode, int value)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, value));
        }

        internal void InsertBefore(OpCode opCode, MethodReference method)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, method));
        }

        internal void InsertBefore(OpCode opCode, Instruction target)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, target));
        }

        internal void InsertBefore(OpCode opCode, TypeReference type)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, type));
        }
    }

    internal class Injector
    {
        static void InjectPropertySetMethod(ModuleDefinition module, TypeDefinition type, PropertyDefinition property)
        {
            var worker = new Worker(property.SetMethod, "");
            worker.InsertBefore(OpCodes.Ldarg_0);
            worker.InsertBefore(OpCodes.Ldarg_0);
            worker.InsertBefore(OpCodes.Ldstr, property.Name);
        }
        /// <summary>
        /// 为方法注入对应的标记
        /// </summary>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        static void InjectMethod(ModuleDefinition module, TypeDefinition type, MethodDefinition method)
        {
            foreach (var s in method.Body.Instructions)
            {
                UnityEngine.Debug.Log($"{s.OpCode}:{s.Operand}");
            }
            return;
            var methodName = $"{type.FullName}.{method.Name}";
            var hotfixExtisMethod = module.ImportReference(typeof(int).GetMethod("Exists", new[] { typeof(string) }));
            var hotfixInvokeMethod = module.ImportReference(typeof(int).GetMethod("Invoke", new[] { typeof(string), typeof(string), typeof(string), typeof(object), typeof(object[]) }));

            var worker = new Worker(method, methodName);
            if (worker.HasMark())
            {
                return;
            }

            worker.InsertBefore(OpCodes.Ldarg_0);
            worker.InsertBefore(OpCodes.Ldstr, method.Name);
            worker.InsertBefore(OpCodes.Call, hotfixInvokeMethod);

            //var extis = Hotfix.Extis(key)
            worker.InsertBefore(OpCodes.Nop);
            worker.InsertBefore(OpCodes.Ldstr, methodName);
            worker.InsertBefore(OpCodes.Call, hotfixExtisMethod);
            worker.InsertBefore(OpCodes.Stloc_0);
            worker.InsertBefore(OpCodes.Ldloc_0);

            //if(extis == false) jump to the under code
            worker.InsertBefore(OpCodes.Brfalse_S, worker.insertPoint);

            //else Hotfix.Invoke(key, typeName, methodName, instance, args)
            worker.InsertBefore(OpCodes.Nop);
            worker.InsertBefore(OpCodes.Ldstr, methodName);
            worker.InsertBefore(OpCodes.Ldstr, type.FullName);
            worker.InsertBefore(OpCodes.Ldstr, method.Name);

            //如果这个方法是静态instance为空否则传入自己
            worker.InsertBefore(method.IsStatic ? OpCodes.Ldnull : OpCodes.Ldarg_0);

            //构造参数列表
            var paramsCount = method.Parameters.Count;
            worker.InsertBefore(OpCodes.Ldc_I4, paramsCount);
            worker.InsertBefore(OpCodes.Newarr, module.ImportReference(typeof(Object)));
            for (int i = 0; i < paramsCount; i++)
            {
                var argIndex = method.IsStatic ? i : i + 1;
                worker.InsertBefore(OpCodes.Dup);
                worker.InsertBefore(OpCodes.Ldc_I4, i);
                var paramType = method.Parameters[i].ParameterType;
                worker.InsertBefore(OpCodes.Ldarg, argIndex);
                if (paramType.IsValueType)
                {
                    worker.InsertBefore(OpCodes.Box, paramType);
                }
                worker.InsertBefore(OpCodes.Stelem_Ref);
            }

            //调用修复后的方法
            worker.InsertBefore(OpCodes.Call, hotfixInvokeMethod);
            var methodReturnVoid = method.ReturnType.FullName.Equals("System.Void");
            if (methodReturnVoid)
            {
                worker.InsertBefore(OpCodes.Pop);
            }
            else
            {
                worker.InsertBefore(OpCodes.Unbox_Any, method.ReturnType);
            }

            //return
            worker.InsertBefore(OpCodes.Ret);
            worker.OffsetMethod();
        }


        public static void Inject(string assemblyPath)
        {
            var readerParameters = new ReaderParameters(ReadingMode.Deferred);
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters);
            if (assembly == null)
            {
                return;
            }

            try
            {
                var module = assembly.MainModule;
                foreach (var type in module.Types)
                {
                    UnityEngine.Debug.Log("---------------------------");
                    foreach(var method in type.Methods)
                    {
                        if(method.Name == "Initialize")
                        {
                            InjectMethod(module, type, method);
                        }
                    }
                    //foreach (var property in type.Properties)
                    //{
                    //    InjectMethod(module, type, property.SetMethod);
                    //}
                }
                assembly.Write(assemblyPath, new WriterParameters { WriteSymbols = true });
            }
            catch (Exception ex)
            {
                throw new Exception($"Inject failed {ex}");
            }
            finally
            {
                if (assembly.MainModule.SymbolReader != null)
                {
                    assembly.MainModule.SymbolReader.Dispose();
                }
            }
        }
    }
}
