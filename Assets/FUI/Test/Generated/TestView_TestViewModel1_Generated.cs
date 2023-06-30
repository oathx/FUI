using FUI.Bindable;
using FUI.UGUI;
using FUI.UGUI.VisualElement;

using System;
using System.Collections.Generic;

namespace FUI.Test
{
    /// <summary>
    /// 这些地方的代码应该由rosyln在编译时生成并注入
    /// </summary>
    public class FUI_Test_TestView__FUI_Test_TestViewModel1_Binding_Generated : UGUIView
    {
        public FUI_Test_TestView__FUI_Test_TestViewModel1_Binding_Generated(ViewModel bindingContext, IAssetLoader assetLoader, string assetPath, string viewName) : base(bindingContext, assetLoader, assetPath, viewName) { }

        public FUI_Test_TestView__FUI_Test_TestViewModel1_Binding_Generated(ViewModel bindingContext, IAssetLoader assetLoader, UnityEngine.GameObject gameObject, string viewName) : base(bindingContext, assetLoader, gameObject, viewName) { }

        protected override Dictionary<string, string> LoadBindingConfig()
        {
            return new Dictionary<string, string>()
            {
                {"txt_Name", "Name" },
                {"txt_ID", "ID" },
                {"txt_Age", "Age" },
                {"img_Icon", "ID" },
                {"btn_submit", "Submit" }
            };
        }

        delegate void PropertyChangedDelegate(object value);
        delegate void PropertyChangedDelegate<T>(T value);

        PropertyChangedDelegate NameChanged;
        PropertyChangedDelegate<int> IDChanged;
        PropertyChangedDelegate<int> AgeChanged;
        PropertyChangedDelegate<Action> SubmitChanged;

        public override void Binding(ObservableObject bindingContext)
        {
            base.Binding(bindingContext);
            NameChanged += (GetVisualElement<ObjectToText>("Name", "txt_Name") as IVisualElement).OnValueChanged;
            IDChanged += (GetVisualElement<IntToTextConverter>("ID", "txt_ID") as IVisualElement<int>).OnValueChanged;
            AgeChanged += (GetVisualElement<IntToTextConverter>("Age", "txt_Age") as IVisualElement<int>).OnValueChanged;
            IDChanged += (GetVisualElement<IntToFormatImageConverter>("ID", "img_Icon") as IVisualElement<int>).OnValueChanged;
            SubmitChanged += (GetVisualElement<ActionToButtonEventConverter>("Submit", "btn_submit") as IVisualElement<Action>).OnValueChanged;
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
            var context = BindingContext as TestViewModel;
            switch (propertyName)
            {
                case "Name":
                    NameChanged?.Invoke(context.Name);
                    break;
                case "ID":
                    IDChanged?.Invoke(context.ID);
                    break;
                case "Age":
                    AgeChanged?.Invoke(context.Age);
                    break;
                case "Submit":
                    SubmitChanged?.Invoke(context.Submit);
                    break;
            }
        }
    }
}