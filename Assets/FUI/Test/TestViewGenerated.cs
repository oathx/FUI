
using System.Reflection;

using UnityEngine;

namespace FUI.Test
{
    public class TestViewGenerated : View
    {
        GameObject gameObject;

        public TestViewGenerated(ViewModel bindingContext) : base(bindingContext)
        {
            var asset = Resources.Load<GameObject>("TestView");
            this.gameObject = Object.Instantiate(asset);

            AddValueConverter();
        }

        //TODO 后续将公用部分提成基类 UGUIView
        void AddValueConverter()
        {
            //TODO  这个配置到时候直接在prefab上操作 序列化到prefab里面
            var config = new System.Collections.Generic.Dictionary<string, string>()
            {
                {"txt_Name", "Name" },
                {"txt_ID", "ID" },
                {"txt_Age", "Age" },
                {"img_Icon", "ID" },
                {"btn_submit", "Submit" }
            };

            foreach(var item in gameObject.transform.GetComponentsInChildren<Transform>(true))
            {
                var converter = item.GetComponent<IValueConverter>();
                if(converter == null)
                {
                    continue;
                }

                var bindingPropertyName = config[item.name];
                AddConverter(bindingPropertyName, converter);
            }
        }

        protected override void OnPropertyChanged(object sender, string propertyName)
        {
            //TODO  这个地方存在拆装箱和反射调用  后续考虑通过rosyln注入代码解决
            switch (propertyName)
            {
                case "Name":
                    var name = GetValue<string>(propertyName);
                    Convert(propertyName, name);
                    break;
                case "ID":
                    var id = GetValue<int>(propertyName);
                    Convert(propertyName, id);
                    break;
                case "Age":
                    var age = GetValue<int>(propertyName);
                    Convert(propertyName, age);
                    break;
                case "Submit":
                    UnityEngine.Debug.Log($"OnPropertyChanged:{propertyName}  value:{GetValue<System.Action>(propertyName)}");
                    var submit = GetValue<System.Action>(propertyName);
                    Convert(propertyName, submit);
                    break;
            }
        }

        T GetValue<T>(string propertyName)
        {
            var flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var value = bindingContext.GetType().GetProperty(propertyName, flag).GetValue(bindingContext);
            return (T)value;
        }
    }
}