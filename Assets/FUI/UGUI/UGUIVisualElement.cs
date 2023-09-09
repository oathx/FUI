﻿using UnityEngine;

namespace FUI.UGUI
{
    /// <summary>
    /// 适用于UGUI的视觉元素
    /// </summary>
    public abstract class UGUIVisualElement : MonoBehaviour, IVisualElement
    {
        /// <summary>
        /// 资源加载器
        /// </summary>
        protected IAssetLoader AssetLoader { get; private set; }

        /// <summary>
        /// 设置资源加载器
        /// </summary>
        /// <param name="assetLoader"></param>
        internal void SetAssetLoader(IAssetLoader assetLoader)
        {
            this.AssetLoader = assetLoader;
        }

        /// <summary>
        /// 当绑定的值更改的时候
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetValue(object value);
    }

    /// <summary>
    /// 适用于UGUI的视觉元素
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    public abstract class UGUIVisualElement<TValue> : UGUIVisualElement, IVisualElement<TValue>
    {
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value">值</param>
        public abstract void SetValue(TValue value);

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value">值</param>
        /// <exception cref="System.Exception"></exception>
        public override void SetValue(object value)
        {
            if(!(value is TValue genericValue))
            {
                throw new System.Exception($"can not convert {value.GetType()} to {typeof(TValue)}");
            }

            SetValue(genericValue);
        }
    }
}