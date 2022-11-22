using System.Collections.Generic;

using FUI.Bindable;

namespace FUI
{
    public abstract class View
    {
        /// <summary>
        /// 当前绑定的上下文
        /// </summary>
        protected ObservableObject BindingContext { get; private set; }

        /// <summary>
        /// 所有的视觉元素
        /// </summary>
        Dictionary<string, List<IVisualElement>> visualElements;

        /// <summary>
        /// 初始化这个View
        /// </summary>
        public View()
        {
            visualElements = new Dictionary<string, List<IVisualElement>>();
        }

        /// <summary>
        /// 通过一个上下文初始化这个View
        /// </summary>
        /// <param name="bindingContext"></param>
        public View(ObservableObject bindingContext):this()
        {
            Binding(bindingContext);
        }

        /// <summary>
        /// 当绑定的上下文属性更改的时候
        /// </summary>
        /// <param name="sender">绑定的上下文</param>
        /// <param name="propertyName">更改的属性名</param>
        protected abstract void OnPropertyChanged(object sender, string propertyName);

        /// <summary>
        /// 绑定一个上下文
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <exception cref="System.Exception"></exception>
        public void Binding(ObservableObject bindingContext)
        {
            if(this.BindingContext != null)
            {
                throw new System.Exception("bindingContext not null, you must unbinding before binding");
            }

            if(bindingContext == null)
            {
                throw new System.Exception("binding error, bindingContext is null");
            }

            this.BindingContext = bindingContext;
            this.BindingContext.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// 解绑上下文
        /// </summary>
        public void Unbinding()
        {
            if(this.BindingContext == null)
            {
                return;
            }

            this.BindingContext.PropertyChanged -= OnPropertyChanged;
            this.BindingContext = null;
        }

        /// <summary>
        /// 添加一个值转换器
        /// </summary>
        /// <param name="propertyName">要转换的属性名</param>
        /// <param name="converter">转换器</param>
        protected void AddVisualElement(string propertyName, IVisualElement visualElement)
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
        }

        /// <summary>
        /// 移除一个值转换器
        /// </summary>
        /// <param name="propertyName">转换的属性名</param>
        /// <param name="converter">转换器</param>
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
        }

        /// <summary>
        /// 移除一个值所绑定的视觉元素
        /// </summary>
        /// <param name="propertyName">转换的属性名</param>
        protected void RemoveVisualElement(string propertyName)
        {
            if(!visualElements.TryGetValue(propertyName, out _))
            {
                return;
            }
            visualElements.Remove(propertyName);
        }

        /// <summary>
        /// 移除所有的视觉元素
        /// </summary>
        protected void RemoveVisualElements()
        {
            visualElements.Clear();
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
                (element as IVisualElement<T>).OnValueChanged(value);
            }
        }
    }
}
