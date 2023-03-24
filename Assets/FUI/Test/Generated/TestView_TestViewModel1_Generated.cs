using FUI.UGUI;

using System.Collections.Generic;

namespace FUI.Test
{
    /// <summary>
    /// 这些地方的代码应该由rosyln在编译时生成并注入
    /// </summary>
    public class TestView_TestViewModel1_Generated : UGUIView
    {
        public TestView_TestViewModel1_Generated(ViewModel bindingContext, IAssetLoader assetLoader, string assetPath) : base(bindingContext, assetLoader, assetPath) { }

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

        protected override void OnPropertyChanged(object sender, string propertyName)
        {
            var context = BindingContext as TestViewModel;
            switch (propertyName)
            {
                case "Name":
                    PropertyChanged(propertyName, (object)context.Name);
                    break;
                case "ID":
                    PropertyChanged(propertyName, context.ID);
                    break;
                case "Age":
                    PropertyChanged(propertyName, context.Age);
                    break;
                case "Submit":
                    PropertyChanged(propertyName, context.Submit);
                    break;
            }
        }
    }
}