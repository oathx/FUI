using FUI.Bindable;
using FUI.UGUI;
using FUI.UGUI.VisualElement;

using System;

namespace FUI.Test
{
    internal class TestView_Binding_Generated : UGUIView
    {
        public TestView_Binding_Generated(ViewModel bindingContext, IAssetLoader assetLoader, string assetPath, string viewName) : base(bindingContext, assetLoader, assetPath, viewName) { }

        public TestView_Binding_Generated(ViewModel bindingContext, IAssetLoader assetLoader, UnityEngine.GameObject gameObject, string viewName) : base(bindingContext, assetLoader, gameObject, viewName) { }

        delegate void PropertyChangedDelegate(object value);
        delegate void PropertyChangedDelegate<T>(T value);

        PropertyChangedDelegate NameChanged;
        PropertyChangedDelegate<int> IDChanged;
        PropertyChangedDelegate<int> AgeChanged;
        PropertyChangedDelegate<Action> SubmitChanged;

        public override void Binding(ObservableObject bindingContext)
        {
            base.Binding(bindingContext);
            NameChanged += (GetVisualElement<ObjectToText>(@"txt_Name") as IVisualElement).OnValueChanged;
            IDChanged += (GetVisualElement<IntToTextConverter>(@"txt_ID") as IVisualElement<int>).OnValueChanged;
            AgeChanged += (GetVisualElement<IntToTextConverter>(@"txt_Age") as IVisualElement<int>).OnValueChanged;
            IDChanged += (GetVisualElement<IntToFormatImageConverter>(@"img_Icon") as IVisualElement<int>).OnValueChanged;
            SubmitChanged += (GetVisualElement<ActionToButtonEventConverter>(@"btn_submit") as IVisualElement<Action>).OnValueChanged;
        }

        public override void Unbinding()
        {
            base.Unbinding();
            NameChanged = null;
            IDChanged = null;
            AgeChanged = null;
            SubmitChanged = null;
        }

        protected override void OnPropertyChanged(object sender, string propertyName)
        {
            if(BindingContext is TestViewModel testViewModel)
            {
                switch(propertyName)
                {
                    case @"Name":
                        NameChanged?.Invoke(testViewModel.Name);
                        break;
                    case @"ID":
                        IDChanged?.Invoke(testViewModel.ID);
                        break;
                    case @"Age":
                        AgeChanged?.Invoke(testViewModel.Age);
                        break;
                    case @"Submit":
                        SubmitChanged?.Invoke(testViewModel.Submit);
                        break;
                }
                return;
            }

            var context = BindingContext as ViewModel;
            switch(propertyName)
            {
                case @"Name":
                    NameChanged?.Invoke(context.GetProperty<string>("Name"));
                    break;
                case @"ID":
                    IDChanged?.Invoke(context.GetProperty<int>("ID"));
                    break;
                case @"Age":
                    AgeChanged?.Invoke(context.GetProperty<int>("Age"));
                    break;
                case @"Submit":
                    SubmitChanged?.Invoke(context.GetProperty<Action>("Submit"));
                    break;
            }
        }

        void SetProperty<T>(string propertyName, T value)
        {
            if(BindingContext is TestViewModel testViewModel)
            {
                if(propertyName == @"Name" && value is Name name)
                {
                    testViewModel.Name = name;
                    return;
                }

                if(propertyName == @"ID" && value is int id)
                {
                    testViewModel.ID = id;
                    return;
                }
            }
        }
    }
}
