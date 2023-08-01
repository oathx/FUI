namespace FUISourcesGenerator
{
    public partial class DataBindingContextGenerator
    {
        const string viewNameMark = "*ViewName*";
        const string propertyChengedMark = "*PropertyChangedDelegates*";
        const string bindingMark = "*Binding*";
        const string unbindingMark = "*Unbinding*";
        const string onPropertyChangedMark = "*OnPropertyChanged*";
        const string viewModelTypeMark = "*ViewModelType*";
        const string viewModelNameMark = "*ViewModelName*";
        const string propertyChangedCasesMark = "*PropertyChangedCases*";


        const string Template = @"
namespace __DataBindingGenerated
{
    public class __*ViewName*_Binding_Generated : FUI.UGUI.UGUIView
    {
        public *ViewName*_Binding_Generated(FUI.ViewModel bindingContext, FUI.IAssetLoader assetLoader, string assetPath, string viewName) : base(bindingContext, assetLoader, assetPath, viewName) { }

        public *ViewName*_Binding_Generated(FUI.ViewModel bindingContext, FUI.IAssetLoader assetLoader, UnityEngine.GameObject gameObject, string viewName) : base(bindingContext, assetLoader, gameObject, viewName) { }

        delegate void PropertyChangedDelegate(object value);
        delegate void PropertyChangedDelegate<T>(T value);

*PropertyChangedDelegates*

        public override void Binding(FUI.Bindable.ObservableObject bindingContext)
        {
            base.Binding(bindingContext);
*Binding*
        }

        public override void Unbinding()
        {
            base.Unbinding();
*Unbinding*
        }

        protected override void OnPropertyChanged(object sender, string propertyName)
        {
*OnPropertyChanged*
        }
    }
}
";

        const string OnPropertyChangedTemplate = @"
            if(BindingContext is *ViewModelType* *ViewModelName*)
            {
                switch(propertyName)
                {
                    *PropertyChangedCases*
                }
                return;
            }
";
    }
}
