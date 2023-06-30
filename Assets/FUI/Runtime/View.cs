using FUI.Bindable;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FUI
{
    public abstract class View
    {
        #region 视觉元素查找键
        /// <summary>
        /// 视觉元素_绑定的属性名_视觉元素类型_查找键
        /// </summary>
        struct VE_PN_T_KEY
        {
            public readonly string propertyName;
            public readonly string elementType;

            public VE_PN_T_KEY(string propertyName, string elementType)
            {
                this.propertyName = propertyName;
                this.elementType = elementType;
            }

            public VE_PN_T_KEY(string propertyName, IVisualElement visualElement)
            {
                this.propertyName = propertyName;
                this.elementType = visualElement.GetType().FullName;
            }

            public override int GetHashCode()
            {
                return propertyName.GetHashCode() ^ elementType.GetHashCode();
            }
        }

        /// <summary>
        /// 视觉元素唯一键
        /// </summary>
        struct VE_U_KEY
        {
            /// <summary>
            /// 绑定的属性名
            /// </summary>
            public readonly string propertyName;
            /// <summary>
            /// 视觉元素的名字
            /// </summary>
            public readonly string elementName;
            /// <summary>
            /// 视觉元素的类型
            /// </summary>
            public readonly string elementType;

            public VE_U_KEY(string propertyName, string elementName, string elementType)
            {
                this.propertyName = propertyName;
                this.elementName = elementName;
                this.elementType = elementType;
            }

            public VE_U_KEY(string propertyName, string elementName, IVisualElement visualElement)
            {
                this.propertyName = propertyName;
                this.elementName = elementName;
                this.elementType = visualElement.GetType().FullName;
            }

            public override int GetHashCode()
            {
                return propertyName.GetHashCode() ^ elementName.GetHashCode() ^ elementType.GetHashCode();
            }

            public override string ToString()
            {
                return $"PropertyName:{propertyName} elementName:{elementName} elementType:{elementType}";
            }
        }
        #endregion

        /// <summary>
        /// 当前绑定的上下文
        /// </summary>
        protected ObservableObject BindingContext { get; private set; }

        /// <summary>
        /// 界面名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 视觉元素唯一查找表
        /// </summary>
        Dictionary<VE_U_KEY, IVisualElement> VE_U_MAP;

        /// <summary>
        /// 视觉元素属性名映射表
        /// </summary>
        Dictionary<string, List<IVisualElement>> VE_PN_MAP;

        /// <summary>
        /// 视觉元素 属性名加类型 映射表
        /// </summary>
        Dictionary<VE_PN_T_KEY, List<IVisualElement>> VE_PN_T_MAP;

        /// <summary>
        /// 初始化这个View
        /// </summary>
        public View(string name)
        {
            this.Name = name;
            VE_U_MAP = new Dictionary<VE_U_KEY, IVisualElement>();
            VE_PN_MAP = new Dictionary<string, List<IVisualElement>>();
            VE_PN_T_MAP = new Dictionary<VE_PN_T_KEY, List<IVisualElement>>();
        }

        /// <summary>
        /// 通过一个上下文初始化这个View
        /// </summary>
        /// <param name="bindingContext"></param>
        public View(ObservableObject bindingContext, string name) : this(name)
        {
            Binding(bindingContext);
        }

        /// <summary>
        /// 当绑定的上下文属性更改的时候
        /// </summary>
        /// <param name="sender">绑定的上下文</param>
        /// <param name="propertyName">更改的属性名</param>
        protected virtual void OnPropertyChanged(object sender, string propertyName) { }

        /// <summary>
        /// 绑定一个上下文
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <exception cref="System.Exception"></exception>
        public virtual void Binding(ObservableObject bindingContext)
        {
            if(this.BindingContext != null)
            {
                throw new System.Exception($"{Name} binding error: bindingContext not null, you must unbinding first");
            }

            this.BindingContext = bindingContext ?? throw new System.Exception($"{Name} binding error: bindingContext is null");
            this.BindingContext.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// 解绑上下文
        /// </summary>
        public virtual void Unbinding()
        {
            if(this.BindingContext == null)
            {
                return;
            }

            this.BindingContext.PropertyChanged -= OnPropertyChanged;
            this.BindingContext = null;
        }

        /// <summary>
        /// 添加一个视觉元素
        /// </summary>
        /// <param name="propertyName">这个视觉元素绑定的属性名</param>
        /// <param name="elementName">这个视觉元素的名字</param>
        /// <param name="visualElement">要添加的视觉元素</param>
        protected void AddVisualElement(string propertyName, string elementName, IVisualElement visualElement)
        {
            var uniqueKey = new VE_U_KEY(propertyName, elementName, visualElement);
            if (VE_U_MAP.ContainsKey(uniqueKey))
            {
                UnityEngine.Debug.LogWarning($"{Name} already contains uniqueKey {uniqueKey} will replace it");
            }
            VE_U_MAP[uniqueKey] = visualElement;

            if (!VE_PN_MAP.TryGetValue(propertyName, out var elementList))
            {
                elementList = new List<IVisualElement> { visualElement };
                VE_PN_MAP[propertyName] = elementList;
            }
            else
            {
                elementList.Add(visualElement);
            }

            var propertyNameKey = new VE_PN_T_KEY(propertyName, visualElement);
            if(!VE_PN_T_MAP.TryGetValue(propertyNameKey, out var elementList2))
            {
                elementList2 = new List<IVisualElement> { visualElement };
                VE_PN_T_MAP[propertyNameKey] = elementList2;
            }
            else
            {
                elementList2.Add(visualElement);
            }
        }

        /// <summary>
        /// 移除一个视觉元素
        /// </summary>
        /// <param name="visualElement">要移除的视觉元素</param>
        protected void RemoveVisualElement(IVisualElement visualElement)
        {
            VE_U_KEY? uniqueKey = null;
            foreach(var kv in VE_U_MAP)
            {
                if(kv.Value == visualElement)
                {
                    uniqueKey = kv.Key;
                    VE_U_MAP.Remove(kv.Key);
                    break;
                }
            }

            if(uniqueKey == null)
            {
                return;
            }

            if(VE_PN_MAP.TryGetValue(uniqueKey.Value.propertyName, out var elementList))
            {
                elementList.Remove(visualElement);
            }

            var pn_t_key = new VE_PN_T_KEY(uniqueKey.Value.propertyName, visualElement);
            if(VE_PN_T_MAP.TryGetValue(pn_t_key, out elementList))
            {
                elementList.Remove(visualElement);
            }
        }

        /// <summary>
        /// 移除所有的视觉元素
        /// </summary>
        protected void RemoveVisualElements()
        {
            VE_U_MAP.Clear();
            VE_PN_MAP.Clear();
            VE_PN_T_MAP.Clear();
        }

        /// <summary>
        /// 获取一个视觉元素
        /// </summary>
        /// <typeparam name="TVisualElement">视觉元素类型</typeparam>
        /// <param name="propertyName">这个视觉元素绑定的属性名</param>
        /// <param name="elementName">元素名</param>
        /// <returns></returns>
        protected TVisualElement GetVisualElement<TVisualElement>(string propertyName, string elementName) where TVisualElement : IVisualElement
        {
            var uniqueKey = new VE_U_KEY(propertyName, elementName, typeof(TVisualElement).FullName);
            if(!VE_U_MAP.TryGetValue(uniqueKey, out var visualElement))
            {
                return default;
            }
            return (TVisualElement)visualElement;
        }

        /// <summary>
        /// 获取绑定某个属性的所有元素
        /// </summary>
        /// <param name="propertyName">绑定的属性名</param>
        /// <returns>元素集合</returns>
        protected IEnumerable<IVisualElement> GetVisualElements(string propertyName)
        {
            if(!VE_PN_MAP.TryGetValue(propertyName, out var elements))
            {
                return null;
            }

            return elements;
        }

        /// <summary>
        /// 获取绑定某个属性的所有元素
        /// </summary>
        /// <typeparam name="TValue">绑定的属性类型</typeparam>
        /// <param name="propertyName">绑定的属性名</param>
        /// <returns>元素集合</returns>
        protected IEnumerable<IVisualElement<TValue>> GetVisualElements<TValue>(string propertyName)
        {
            if (!VE_PN_MAP.TryGetValue(propertyName, out var elements))
            {
                return null;
            }
            return elements.Cast<IVisualElement<TValue>>();
        }

        protected IVisualElement<TValue> GetVisualElement<TVisualElement, TValue>(string propertyName) where TVisualElement : IVisualElement<TValue>
        {
            var key = new VE_PN_T_KEY(propertyName, typeof(TValue).FullName);
            if(VE_PN_T_MAP.TryGetValue(key, out var element))
            {
                return (IVisualElement<TValue>)element;
            }
            return null;
        }

        /// <summary>
        /// 将一个绑定的属性应用到对应的视觉元素
        /// </summary>
        /// <param name="propertyName">绑定的属性名</param>
        /// <param name="value">绑定的属性值</param>
        protected void PropertyChanged(string propertyName, object value)
        {
            var elements = GetVisualElements(propertyName);
            if (elements == null)
            {
                return;
            }

            foreach (var element in elements)
            {
                element.OnValueChanged(value);
            }
        }

        /// <summary>
        /// 将一个绑定的属性应用到对应的视觉元素
        /// </summary>
        /// <typeparam name="TVisualElement">视觉元素类型</typeparam>
        /// <param name="propertyName">绑定的属性名</param>
        /// <param name="value">绑定的属性值</param>
        protected void PropertyChanged<TVisualElement>(string propertyName, object value) where TVisualElement : IVisualElement 
        {
            if (!VE_PN_MAP.TryGetValue(propertyName, out var elements))
            {
                return;
            }
            foreach (var element in elements)
            {
                if (!(element is TVisualElement))
                {
                    continue;
                }
                element.OnValueChanged(value);
            }
        }

        /// <summary>
        /// 将一个绑定的属性应用到对应的视觉元素
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="propertyName">绑定的属性名</param>
        /// <param name="value">绑定的属性值</param>
        protected void PropertyChanged<T>(string propertyName, T value)
        {
            if(!VE_PN_MAP.TryGetValue(propertyName, out var elements))
            {
                return;
            }

            foreach(var element in elements)
            { 
                if(!(element is IVisualElement<T> genericElement))
                {
                    continue;
                }
                genericElement.OnValueChanged(value);
            }
        }

        /// <summary>
        /// 将一个绑定的属性应用到对应的视觉元素
        /// </summary>
        /// <typeparam name="TVisualElement">视觉元素类型</typeparam>
        /// <typeparam name="TValue">绑定的属性类型</typeparam>
        /// <param name="propertyName">绑定的属性名</param>
        /// <param name="value">绑定的属性值</param>
        protected void PropertyChanged<TVisualElement, TValue>(string propertyName, TValue value) where TVisualElement : IVisualElement<TValue>
        {
            if (!VE_PN_MAP.TryGetValue(propertyName, out var elements))
            {
                return;
            }
            foreach (var element in elements)
            {
                if(!(element is TVisualElement genericElement))
                {
                    continue;
                }
                genericElement.OnValueChanged(value);
            }
        }
    }
}
