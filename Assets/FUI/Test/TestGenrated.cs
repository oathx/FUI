namespace FUI.Test
{
    public partial class TestViewModel
    {
        public FUI.Bindable.PropertyChangedHandler<FUI.Test.Name> _Name_Changed;
        public FUI.Bindable.PropertyChangedHandler<System.Int32> _ID_Changed;
        public FUI.Bindable.PropertyChangedHandler<System.Int32> _Age_Changed;
        public FUI.Bindable.PropertyChangedHandler<System.Action> _Submit_Changed;
        public FUI.Bindable.PropertyChangedHandler<FUI.Bindable.ObservableList<FUI.Test.ItemData>> _List_Changed;
    }
}
namespace __DataBindingGenerated
{
    public class __TestView_Binding_Generated : FUI.UGUI.UGUIView
    {
        FUI.UGUI.ValueConverter.ObjectToStringConverter FUI_UGUI_ValueConverter_ObjectToStringConverter = new FUI.UGUI.ValueConverter.ObjectToStringConverter();
        FUI.UGUI.ValueConverter.IntToStringConverter FUI_UGUI_ValueConverter_IntToStringConverter = new FUI.UGUI.ValueConverter.IntToStringConverter();
        public __TestView_Binding_Generated(FUI.ViewModel bindingContext, FUI.IAssetLoader assetLoader, string assetPath, string viewName) : base(bindingContext, assetLoader, assetPath, viewName)
        {
        }

        public __TestView_Binding_Generated(FUI.ViewModel bindingContext, FUI.IAssetLoader assetLoader, UnityEngine.GameObject gameObject, string viewName) : base(bindingContext, assetLoader, gameObject, viewName)
        {
        }

        public override void Binding(FUI.Bindable.ObservableObject bindingContext)
        {
            base.Binding(bindingContext);
            if (BindingContext is FUI.Test.TestViewModel FUI_Test_TestViewModel)
            {
                FUI_Test_TestViewModel._Name_Changed += (sender, preValue, @value) =>
                {
                    var convertedTempValue = FUI_UGUI_ValueConverter_ObjectToStringConverter.Convert(@value);
                    if (!(convertedTempValue is System.String convertedValue))
                    {
                        throw new System.Exception("FUI.Test.TestViewModel.Name can not convert to System.String property:FUI.Test.Name converter:FUI.UGUI.ValueConverter.ObjectToStringConverter(FUI.Test.Name,System.String) element:FUI.UGUI.VisualElement.TextElement(System.String)");
                    }

                    var element = GetVisualElement<FUI.UGUI.VisualElement.TextElement>("txt_Name");
                    if (element == null)
                    {
                        throw new System.Exception($"{this.Name} GetVisualElement type:{typeof(FUI.UGUI.VisualElement.TextElement)} path:{@"txt_Name"} failed");
                    }

                    element.SetValue(convertedValue);
                };
                FUI_Test_TestViewModel._ID_Changed += (sender, preValue, @value) =>
                {
                    var convertedTempValue = FUI_UGUI_ValueConverter_IntToStringConverter.Convert(@value);
                    if (!(convertedTempValue is System.String convertedValue))
                    {
                        throw new System.Exception("FUI.Test.TestViewModel.ID can not convert to System.String property:System.Int32 converter:FUI.UGUI.ValueConverter.IntToStringConverter(System.Int32,System.String) element:FUI.UGUI.VisualElement.TextElement(System.String)");
                    }

                    var element = GetVisualElement<FUI.UGUI.VisualElement.TextElement>("txt_ID");
                    if (element == null)
                    {
                        throw new System.Exception($"{this.Name} GetVisualElement type:{typeof(FUI.UGUI.VisualElement.TextElement)} path:{@"txt_ID"} failed");
                    }

                    element.SetValue(convertedValue);
                };
                FUI_Test_TestViewModel._Age_Changed += (sender, preValue, @value) =>
                {
                    var convertedTempValue = FUI_UGUI_ValueConverter_IntToStringConverter.Convert(@value);
                    if (!(convertedTempValue is System.String convertedValue))
                    {
                        throw new System.Exception("FUI.Test.TestViewModel.Age can not convert to System.String property:System.Int32 converter:FUI.UGUI.ValueConverter.IntToStringConverter(System.Int32,System.String) element:FUI.UGUI.VisualElement.TextElement(System.String)");
                    }

                    var element = GetVisualElement<FUI.UGUI.VisualElement.TextElement>("txt_Age");
                    if (element == null)
                    {
                        throw new System.Exception($"{this.Name} GetVisualElement type:{typeof(FUI.UGUI.VisualElement.TextElement)} path:{@"txt_Age"} failed");
                    }

                    element.SetValue(convertedValue);
                };
                FUI_Test_TestViewModel._Submit_Changed += (sender, preValue, @value) =>
                {
                    if (!(@value is System.Action convertedValue))
                    {
                        throw new System.Exception("FUI.Test.TestViewModel.Submit can not convert to System.Action property:System.Action element:FUI.UGUI.VisualElement.ButtonEventElement elementValueType:System.Action");
                    }

                    var element = GetVisualElement<FUI.UGUI.VisualElement.ButtonEventElement>("btn_submit");
                    if (element == null)
                    {
                        throw new System.Exception($"{this.Name} GetVisualElement type:{typeof(FUI.UGUI.VisualElement.ButtonEventElement)} path:{@"btn_submit"} failed");
                    }

                    element.SetValue(convertedValue);
                };
                return;
            }
        }

        public override void Unbinding()
        {
            base.Unbinding();
        }

        protected override void OnPropertyChanged(object sender, string propertyName)
        {
        }
    }
}