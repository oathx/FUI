using UnityEngine;

namespace FUI.UGUI
{
    /// <summary>
    /// 适用于UGUI的值转换器
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    public abstract class UGUIValueConverter<T> : MonoBehaviour, IValueConverter<T>, IValueConverter
    {
        public abstract void Convert(T value);

        void IValueConverter.Convert(object value)
        {
            T genericValue;

            try
            {
                genericValue = (T)value;
            }
            catch
            {
                throw new System.Exception($"can not convert object to {typeof(T)}");
            }

            Convert(genericValue);
        }
    }
}