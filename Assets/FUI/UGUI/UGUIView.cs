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
        public UGUIView(GameObject gameObject, ViewModel bindingContext, IAssetLoader assetLoader) : base(bindingContext)
        {
            this.gameObject = gameObject;
            this.assetLoader = assetLoader;
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
            foreach (var item in gameObject.transform.GetComponentsInChildren<Transform>(true))
            {
                var element = item.GetComponent<IVisualElement>();
                if (element == null)
                {
                    continue;
                }

                //如果不是UGUI所对应的视觉元素组件则不进行初始化
                if(!(element is UGUIVisualElement uguiElement))
                {
                    Debug.LogWarning($"{gameObject.name} {item.name} {element} is not UGUIVisualElement");
                    continue;
                }

                uguiElement.SetAssetLoader(assetLoader);
                var bindingPropertyName = config[item.name];
                AddVisualElement(bindingPropertyName, element);
            }
        }
    }
}
