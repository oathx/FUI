using System;
using System.Collections.Generic;
using System.Text;

namespace FUI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class BindingAttribute : Attribute
    {
        public readonly string target;
        public readonly Type adpatorType;

        public BindingAttribute(string target)
        {
            this.target = target;
        }

        public BindingAttribute(string target, Type adpatorType)
        {
            this.target = target;
            this.adpatorType = adpatorType;
        }
    }
}
