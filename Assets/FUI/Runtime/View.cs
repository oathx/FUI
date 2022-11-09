using System.Collections.Generic;

namespace FUI
{
    public abstract class View
    {
        /// <summary>
        /// 当前绑定的上下文
        /// </summary>
        protected readonly ViewModel bindingContext;

        /// <summary>
        /// 所有的值转换器
        /// </summary>
        Dictionary<string, List<IValueConverter>> convertes;

        /// <summary>
        /// 通过一个上下文初始化这个View
        /// </summary>
        /// <param name="bindingContext">绑定的上下文</param>
        public View(ViewModel bindingContext)
        {
            convertes = new Dictionary<string, List<IValueConverter>>();
            this.bindingContext = bindingContext;
            this.bindingContext.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// 当绑定的上下文属性更改的时候
        /// </summary>
        /// <param name="sender">绑定的上下文</param>
        /// <param name="propertyName">更改的属性名</param>
        protected abstract void OnPropertyChanged(object sender, string propertyName);

        /// <summary>
        /// 添加一个值转换器
        /// </summary>
        /// <param name="propertyName">要转换的属性名</param>
        /// <param name="converter">转换器</param>
        protected void AddConverter(string propertyName, IValueConverter converter)
        {
            if(!convertes.TryGetValue(propertyName, out var converterList))
            {
                converterList = new List<IValueConverter> { converter };
                convertes[propertyName] = converterList;
            }
            else
            {
                converterList.Add(converter);
            }
        }

        /// <summary>
        /// 移除一个值转换器
        /// </summary>
        /// <param name="propertyName">转换的属性名</param>
        /// <param name="converter">转换器</param>
        protected void RemoveConverter(string propertyName, IValueConverter converter)
        {
            if(!convertes.TryGetValue(propertyName, out var converterList))
            {
                return;
            }

            if (converterList.Contains(converter))
            {
                converterList.Remove(converter);
            }
        }

        /// <summary>
        /// 移除一个值的所有转换器
        /// </summary>
        /// <param name="propertyName">转换的属性名</param>
        protected void RemoveConverter(string propertyName)
        {
            if(!convertes.TryGetValue(propertyName, out _))
            {
                return;
            }
            convertes.Remove(propertyName);
        }

        /// <summary>
        /// 移除所有的值转换器
        /// </summary>
        protected void RemoveConverter()
        {
            convertes.Clear();
        }

        /// <summary>
        /// 将一个绑定的属性转换成对应的效果
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="propertyName">绑定的属性名</param>
        /// <param name="value">绑定的属性值</param>
        protected void Convert<T>(string propertyName, T value)
        {
            if(!convertes.TryGetValue(propertyName, out var converterList))
            {
                return;
            }

            foreach(var converter in converterList)
            {
                (converter as IValueConverter<T>).Convert(value);
            }
        }
    }
}
