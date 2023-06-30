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
        /// 视觉元素查找键
        /// </summary>
        struct VisualElementPropertyNameKey
        {
            readonly string propertyName;
            readonly Type elementType;

            public VisualElementPropertyNameKey(string propertyName, Type elementType)
            {
                this.propertyName = propertyName;
                this.elementType = elementType;
            }

            public override int GetHashCode()
            {
                return propertyName.GetHashCode() ^ elementType.GetHashCode();
            }
        }

        /// <summary>
        /// 视觉元素唯一键
        /// </summary>
        struct VisualElementUniqueKey
        {
            readonly string propertyName;
            readonly string elementName;
            readonly Type elementType;

            public VisualElementUniqueKey(string propertyName, string elementName, Type elementType)
            {
                this.propertyName = propertyName;
                this.elementName = elementName;
                this.elementType = elementType;
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
        /// 视觉元素属性名映射表
        /// </summary>
        Dictionary<string, List<IVisualElement>> visualElements;

        /// <summary>
        /// 视觉元素 属性名加类型 映射表
        /// </summary>
        Dictionary<VisualElementPropertyNameKey, List<IVisualElement>> visualElementLookup;

        /// <summary>
        /// 视觉元素唯一查找表
        /// </summary>
        Dictionary<VisualElementUniqueKey, IVisualElement> visualElementUniqueLookup;

        /// <summary>
        /// 初始化这个View
        /// </summary>
        public View(string name)
        {
            this.Name = name;
            visualElements = new Dictionary<string, List<IVisualElement>>();
            visualElementLookup = new Dictionary<VisualElementPropertyNameKey, List<IVisualElement>>();
            visualElementUniqueLookup = new Dictionary<VisualElementUniqueKey, IVisualElement>();
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

            if(bindingContext == null)
            {
                throw new System.Exception($"{Name} binding error: bindingContext is null");
            }

            this.BindingContext = bindingContext;
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
        /// <param name="propertyName">要转换的属性名</param>
        /// <param name="visualElement">视觉元素</param>
        protected void AddVisualElement(string propertyName, string elementName, IVisualElement visualElement)
        {
            if(!visualElements.TryGetValue(propertyName, out var elementList))
            {
                elementList = new List<IVisualElement> { visualElement };
                visualElements[propertyName] = elementList;
            }
            else
            {
                elementList.Add(visualElement);
            }

            var propertyNameKey = new VisualElementPropertyNameKey(propertyName, visualElement.GetType());
            if(!visualElementLookup.TryGetValue(propertyNameKey, out var elementList2))
            {
                elementList2 = new List<IVisualElement> { visualElement };
                visualElementLookup[propertyNameKey] = elementList2;
            }
            else
            {
                elementList2.Add(visualElement);
            }

            var uniqueKey = new VisualElementUniqueKey(propertyName, elementName, visualElement.GetType());
            if(visualElementUniqueLookup.ContainsKey(uniqueKey))
            {
                UnityEngine.Debug.LogWarning($"{Name} already contains uniqueKey {uniqueKey} will replace it");
            }
            visualElementUniqueLookup[uniqueKey] = visualElement;
        }

        /// <summary>
        /// 移除一个视觉元素
        /// </summary>
        /// <param name="propertyName">转换的属性名</param>
        /// <param name="visualElement">视觉元素</param>
        protected void RemoveVisualElement(string propertyName, IVisualElement visualElement)
        {
            if(!visualElements.TryGetValue(propertyName, out var elementList))
            {
                return;
            }

            if (elementList.Contains(visualElement))
            {
                elementList.Remove(visualElement);
            }

            var key = new VisualElementPropertyNameKey(propertyName, visualElement.GetType());
            if (visualElementLookup.ContainsKey(key))
            {
                visualElementLookup.Remove(key);
            }
        }

        /// <summary>
        /// 移除一个值所绑定的视觉元素
        /// </summary>
        /// <param name="propertyName">转换的属性名</param>
        protected void RemoveVisualElement(string propertyName)
        {
            if(!visualElements.TryGetValue(propertyName, out var element))
            {
                return;
            }
            visualElements.Remove(propertyName);
            var key = new VisualElementPropertyNameKey(propertyName, element.GetType());
            if (visualElementLookup.ContainsKey(key))
            {
                visualElementLookup.Remove(key);
            }
        }

        /// <summary>
        /// 移除所有的视觉元素
        /// </summary>
        protected void RemoveVisualElements()
        {
            visualElements.Clear();
            visualElementLookup.Clear();
        }

        /// <summary>
        /// 获取绑定某个属性的所有元素
        /// </summary>
        /// <param name="propertyName">绑定的属性名</param>
        /// <returns>元素集合</returns>
        protected IEnumerable<IVisualElement> GetVisualElements(string propertyName)
        {
            if(!visualElements.TryGetValue(propertyName, out var elements))
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
            if (!visualElements.TryGetValue(propertyName, out var elements))
            {
                return null;
            }
            return elements.Cast<IVisualElement<TValue>>();
        }

        /// <summary>
        /// 获取绑定某个属性的所有元素
        /// </summary>
        /// <typeparam name="TVisualElement">元素类型</typeparam>
        /// <param name="propertyName">属性名</param>
        /// <returns>对应元素</returns>
        protected IEnumerable<IVisualElement> GetVisualElements<TVisualElement>(string propertyName) where TVisualElement : IVisualElement
        {
            var key = new VisualElementPropertyNameKey(propertyName, typeof(TVisualElement));
            if(visualElementLookup.TryGetValue(key, out var element))
            {
                return element;
            }
            return null;
        }

        protected IVisualElement<TValue> GetVisualElement<TVisualElement, TValue>(string propertyName) where TVisualElement : IVisualElement<TValue>
        {
            var key = new VisualElementPropertyNameKey(propertyName, typeof(TValue));
            if(visualElementLookup.TryGetValue(key, out var element))
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
            if (!visualElements.TryGetValue(propertyName, out var elements))
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
            if(!visualElements.TryGetValue(propertyName, out var elements))
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
            if (!visualElements.TryGetValue(propertyName, out var elements))
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
