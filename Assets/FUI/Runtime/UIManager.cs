using System.Collections.Generic;

namespace FUI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// 所有没有被销毁的容器
        /// </summary>
        Stack<Container> containers;

        public UIManager()
        {
            containers = new Stack<Container>();
        }

        /// <summary>
        /// 通过默认的ViewModel打开一个View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Open<TView>() where TView : View
        {

        }

        /// <summary>
        /// 通过指定的ViewModel打开一个View
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        public void Open<TView, TViewModel>() where TView : View where TViewModel : ViewModel
        {

        }

        /// <summary>
        /// 返回上一个界面
        /// </summary>
        public void Back()
        {

        }
    }
}
