using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Mono.Cecil;

namespace FUISourcesGenerator
{
    public static class Utility
    {
        /// <summary>
        /// 是否拥有某个自定义特性
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attributeFullName">特性类型全名</param>
        /// <returns></returns>
        public static bool HasCustomAttribute(this ICustomAttributeProvider target, string attributeFullName)
        {
            foreach (var attribute in target.CustomAttributes)
            {
                if (attribute.AttributeType.FullName == attributeFullName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取一个泛型格式化后的名字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(string type)
        {
            var tIndex = type.IndexOf("`");
            if (tIndex < 0)
            {
                return type;
            }

            var pre = type.Substring(0, tIndex);
            var leftIndex = type.IndexOf("<");
            var rightIndex = type.IndexOf(">");
            var genericTypes = type.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
            var genericTypeNames = genericTypes.Split(',');
            var argsType = "<";
            for (int i = 0; i < genericTypeNames.Length; i++)
            {
                argsType += i == genericTypeNames.Length - 1 ? genericTypeNames[i] : $"{genericTypeNames[i]},";
            }
            argsType += ">";
            return $"{pre}{argsType}";
        }

        /// <summary>
        /// 格式化代码
        /// </summary>
        /// <param name="text">代码</param>
        /// <returns></returns>
        public static string NormalizeCode(string text)
        {
            return CSharpSyntaxTree.ParseText(text).GetRoot().NormalizeWhitespace().ToFullString();
        } 

        /// <summary>
        /// 获取属性更改委托名字
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyChangedDelegateName(string propertyName)
        {
            return $"_{propertyName}_Changed";
        }
    }
}
