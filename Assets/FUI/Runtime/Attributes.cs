using System;

namespace FUI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class BindingAttribute : Attribute
    {
        public readonly string target;
        public readonly Type valueConverterType;
        public readonly Type visualElementType;

        /// <summary>
        /// 不指定目标和转换器类型
        /// </summary>
        public BindingAttribute() { }

        /// <summary>
        /// 绑定一个结构到某个View
        /// </summary>
        /// <param name="target"></param>
        public BindingAttribute(string target)
        {
            this.target = target;
        }

        /// <summary>
        /// 绑定一个属性到某个视觉元素
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="visualElementType">视觉元素类型</param>
        public BindingAttribute(string target, Type visualElementType)
        {
            this.target = target;
            this.visualElementType = visualElementType;
        }

        /// <summary>
        /// 绑定一个属性到某个视觉元素
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="valueConverter">值转换器类型</param>
        /// <param name="visualElementType">视觉元素类型</param>
        public BindingAttribute(string target, Type valueConverter, Type visualElementType)
        {
            this.target = target;
            this.visualElementType = visualElementType;
        }
    }
}
