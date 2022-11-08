﻿using Mono.Cecil;

using System.Linq;


namespace FUI.Editor
{
    public static class InjecterExtensions
    {
        /// <summary>
        /// 获取一个属性的定义
        /// </summary>
        /// <param name="type">类型定义</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static PropertyDefinition GetProperty(this TypeDefinition type, string propertyName)
        {
            UnityEngine.Debug.Log($"GetProperty  type:{type}  propertyName:{propertyName}");
            var property = type.Properties.FirstOrDefault(p => p.Name == propertyName);
            if(property != null)
            {
                return property;
            }
            
            if(type.BaseType == null)
            {
                return null;
            }
            UnityEngine.Debug.Log($"baseModule:{type.BaseType.Module.Assembly}");
            UnityEngine.Debug.Log($"baseTypeName:{type.BaseType.FullName}");
            foreach(var t in type.BaseType.Module.Types)
            {
                UnityEngine.Debug.Log(t);
            }
            var baseType = type.BaseType.Resolve();// type.BaseType.Module.GetType(type.BaseType.FullName);
            UnityEngine.Debug.Log($"baseType:{baseType}");
            return GetProperty(baseType, propertyName);
        }

        /// <summary>
        /// 从程序集定义中获取一个类型定义
        /// </summary>
        /// <param name="assembly">程序集定义</param>
        /// <param name="typeFullName">类型名</param>
        /// <returns></returns>
        public static TypeDefinition GetType(this AssemblyDefinition assembly, string typeFullName)
        {
            foreach(var module in assembly.Modules)
            {
                var type = module.GetType(typeFullName);
                if(type != null)
                {
                    return type; 
                }
            }

            return null;
        }

        /// <summary>
        /// 获取一个方法的定义
        /// </summary>
        /// <param name="type">类型定义</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public static MethodDefinition GetMethod(this TypeDefinition type, string methodName)
        {
            return type.Methods.FirstOrDefault(item=>item.Name == methodName);
        }

        /// <summary>
        /// 是否拥有某个自定义特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool HasCustomAttribute<T>(this PropertyDefinition property) where T : System.Attribute
        {
            return property.CustomAttributes.FirstOrDefault(item=>item.AttributeType.FullName == typeof(T).FullName) != null;
        }
    }
}
