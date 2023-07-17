using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FUISourcesGenerator
{
    internal class Injector
    {
        internal Instruction insertPoint
        {
            get { return methodDef.Body.Instructions[insertIndex]; }
        }
        MethodDefinition methodDef;
        ILProcessor processor;
        MethodBody methodBody;
        int insertIndex;

        internal Injector(MethodDefinition methodDefinition, int insertIndex)
        {
            this.methodBody = methodDefinition.Body;
            var processor = methodDefinition.Body.GetILProcessor();
            this.methodDef = methodDefinition;
            this.processor = processor;
            this.insertIndex = insertIndex;
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

        internal void InsertAfter(OpCode opDodes)
        {
            processor.InsertAfter(insertPoint, processor.Create(opDodes));
            insertIndex++;
        }

        internal void InsertBefore(OpCode opCode, string value)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, value));
        }

        internal void InsertAfter(OpCode opDodes, string value)
        {
            processor.InsertAfter(insertPoint, processor.Create(opDodes, value));
            insertIndex++;
        }

        internal void InsertBefore(OpCode opCode, int value)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, value));
        }

        internal void InsertAfter(OpCode opDodes, int value)
        {
            processor.InsertAfter(insertPoint, processor.Create(opDodes, value));
            insertIndex++;
        }

        internal void InsertBefore(OpCode opCode, MethodReference method)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, method));
        }

        internal void InsertAfter(OpCode opDodes, MethodReference method)
        {
            processor.InsertAfter(insertPoint, processor.Create(opDodes, method));
            insertIndex++;
        }

        internal void InsertBefore(OpCode opCode, Instruction target)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, target));
        }

        internal void InsertAfter(OpCode opDodes, Instruction target)
        {
            processor.InsertAfter(insertPoint, processor.Create(opDodes, target));
            insertIndex++;
        }

        internal void InsertBefore(OpCode opCode, TypeReference type)
        {
            processor.InsertBefore(insertPoint, processor.Create(opCode, type));
        }

        internal void InsertAfter(OpCode opDodes, TypeReference type)
        {
            processor.InsertAfter(insertPoint, processor.Create(opDodes, type));
            insertIndex++;
        }
    }
}
