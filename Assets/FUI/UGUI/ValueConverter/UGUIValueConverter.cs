using UnityEngine;

namespace FUI.UGUI
{
    /// <summary>
    /// 适用于UGUI的视觉元素
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    public abstract class UGUIVisualElement<T> : MonoBehaviour, IVisualElement<T>, IVisualElement
    {
        public abstract void OnValueChanged(T value);

        void IVisualElement.OnValueChanged(object value)
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

            OnValueChanged(genericValue);
        }
    }
}