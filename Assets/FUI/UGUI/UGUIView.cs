using System.Collections.Generic;

using UnityEngine;

namespace FUI.UGUI
{
    /// <summary>
    /// 适用于UGUI的view基类
    /// </summary>
    public class UGUIView : View
    {
        protected GameObject gameObject;
        protected IAssetLoader assetLoader;

        /// <summary>
        /// 初始化一个UGUIView
        /// </summary>
        /// <param name="bindingContext">绑定的上下文</param>
        /// <param name="assetLoader">资源加载器</param>
        /// <param name="assetPath">这个view的资源路径</param>
        public UGUIView(ViewModel bindingContext, IAssetLoader assetLoader, string assetPath) : base(bindingContext)
        {
            this.assetLoader = assetLoader;
            this.gameObject = assetLoader.CreateGameObject(assetPath);
            InitializeVisualElements();
        }

        /// <summary>
        /// 初始化一个UGUIView
        /// </summary>
        /// <param name="gameObject">这个view对应的gameobject</param>
        /// <param name="bindingContext">绑定的上下文</param>
        /// <param name="assetLoader">这个view对应的资源加载器</param>
        public UGUIView(ViewModel bindingContext, IAssetLoader assetLoader, GameObject gameObject) : base(bindingContext)
        {
            this.assetLoader = assetLoader;
            this.gameObject = gameObject;
            InitializeVisualElements();
        }

        /// <summary>
        /// 加载对应的绑定配置
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, string> LoadBindingConfig() => null;

        /// <summary>
        /// 初始化这个界面的视觉元素
        /// </summary>
        protected virtual void InitializeVisualElements()
        {
            var config = LoadBindingConfig();
            if(config == null)
            {
                return;
            }

            //获取所有的视觉元素组件
            foreach (var element in gameObject.transform.GetComponentsInChildren<UGUIVisualElement>(true))
            {
                element.SetAssetLoader(assetLoader);
                var bindingPropertyName = config[element.name];
                AddVisualElement(bindingPropertyName, element);
            }
        }
    }
}
