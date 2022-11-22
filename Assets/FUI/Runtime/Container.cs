using System;
using System.Collections.Generic;

namespace FUI
{
    internal class Component
    {
        /// <summary>
        /// 这个组件所对应的View
        /// </summary>
        internal View View { get; private set; }

        /// <summary>
        /// 这个组件所对应的ViewModel
        /// </summary>
        internal ViewModel ViewModel { get; private set; }

        /// <summary>
        /// 这个组件所对应的View的类型
        /// </summary>
        internal Type ViewType { get; private set; }

        /// <summary>
        /// 这个组件所对应的ViewModel的类型
        /// </summary>
        internal Type ViewModelType { get; private set; }

        internal Component(View view, ViewModel viewModel)
        {
            this.View = view;
            this.ViewModel = viewModel;
            this.ViewType = view.GetType();
            this.ViewModelType = viewModel.GetType();
        }

        internal Component(Type viewType, Type viewModelType)
        {
            this.ViewType = viewType;
            this.ViewModelType = viewModelType;
        }

        /// <summary>
        /// 打开这个组件
        /// </summary>
        internal void Open(object param)
        {
            ViewModel.OnOpen(param);
        }

        /// <summary>
        /// 关闭这个组件
        /// </summary>
        internal void Close()
        {
            ViewModel.OnClose();
        }

        /// <summary>
        /// 销毁这个组件
        /// </summary>
        internal void Destroy()
        {
            
        }
    }

    /// <summary>
    /// 界面容器
    /// </summary>
    internal class Container
    {
        public string Name { get; private set; }

        /// <summary>
        /// 组成这个界面的所有组件
        /// </summary>
        List<Component> components;

        internal Container(string name)
        {
            components = new List<Component>();
            this.Name = name;
        }

        /// <summary>
        /// 通过一个组件初始化容器
        /// </summary>
        /// <param name="view">这个组件所对应的View</param>
        /// <param name="viewModel">这个组件所对应的ViewModel</param>
        internal Container(string name, View view, ViewModel viewModel) : this(name)
        {
            var component = new Component(view, viewModel);
            components.Add(component);
        }

        /// <summary>
        /// 给这个容器添加一个组件
        /// </summary>
        /// <param name="view">这个组件所对应的View</param>
        /// <param name="viewModel">这个组件所对应的ViewModel</param>
        internal void AddComponent(View view, ViewModel viewModel)
        {
            components.Add(new Component(view, viewModel));
        }

        /// <summary>
        /// 从容器中移除一个组件
        /// </summary>
        /// <param name="view">这个组件所对应的View</param>
        /// <param name="viewModel">这个组件所对应的ViewModel</param>
        internal void RemoveComponent(View view, ViewModel viewModel)
        {
            components.RemoveAll(item=>item.View == view && item.ViewModel == viewModel);
        }

        /// <summary>
        /// 打开这个容器
        /// </summary>
        internal void Open(object param)
        {
            foreach(var component in components)
            {
                component.Open(param);
            }
        }

        /// <summary>
        /// 关闭这个容器
        /// </summary>
        internal void Close()
        {
            foreach(var component in components)
            {
                component.Close();
            }
        }

        /// <summary>
        /// 销毁这个容器
        /// </summary>
        internal void Destroy()
        {
            foreach(var component in components)
            {
                component.Destroy();
            }
        }
    }
}