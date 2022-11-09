using System;

namespace FUI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class BindingAttribute : Attribute
    {
        public readonly string target;
        public readonly Type converterType;

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
        /// 绑定一个属性到某个值转换器
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="converterType">值转换器类型</param>
        public BindingAttribute(string target, Type converterType)
        {
            this.target = target;
            this.converterType = converterType;
        }
    }
}
