using FUI;
using FUI.UGUI.VisualElement;

using System;

namespace aaa
{
    internal class TestView_Binding_Generated : FUI.UGUI.UGUIView
    {
        public TestView_Binding_Generated(FUI.ViewModel bindingContext, FUI.IAssetLoader assetLoader, string assetPath, string viewName) : base(bindingContext, assetLoader, assetPath, viewName) { }

        public TestView_Binding_Generated(FUI.ViewModel bindingContext, FUI.IAssetLoader assetLoader, UnityEngine.GameObject gameObject, string viewName) : base(bindingContext, assetLoader, gameObject, viewName) { }

        delegate void PropertyChangedDelegate(object value);
        delegate void PropertyChangedDelegate<T>(T value);

        PropertyChangedDelegate NameChanged;
        PropertyChangedDelegate<int> IDChanged;
        PropertyChangedDelegate<int> AgeChanged;
        PropertyChangedDelegate<System.Action> SubmitChanged;

        public override void Binding(FUI.Bindable.ObservableObject bindingContext)
        {
            base.Binding(bindingContext);

            if(bindingContext is FUI.Test.TestViewModel)
            {
                NameChanged += (GetVisualElement<FUI.UGUI.VisualElement.ObjectToText>(@"txt_Name") as FUI.IVisualElement).OnValueChanged;
                IDChanged += (GetVisualElement<IntToTextConverter>(@"txt_ID") as IVisualElement<int>).OnValueChanged;
                AgeChanged += (GetVisualElement<IntToTextConverter>(@"txt_Age") as IVisualElement<int>).OnValueChanged;
                IDChanged += (GetVisualElement<IntToFormatImageConverter>(@"img_Icon") as IVisualElement<int>).OnValueChanged;
                SubmitChanged += (GetVisualElement<ActionToButtonEventConverter>(@"btn_submit") as IVisualElement<Action>).OnValueChanged;
                return;
            }

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
            if(BindingContext is FUI.Test.TestViewModel testViewModel)
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

            var context = BindingContext as FUI.ViewModel;
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
                    SubmitChanged?.Invoke(context.GetProperty<System.Action>("Submit"));
                    break;
            }
        }
    }
}
