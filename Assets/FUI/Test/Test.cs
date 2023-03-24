using System;
using System.Threading.Tasks;

using UnityEngine;

namespace FUI.Test
{
    public class Test : MonoBehaviour
    {
        async void Awake()
        {
            var vm1 = OpenView<TestViewModel>("TestView");
            vm1.Name = new Name { firstName = "test", lastName = "1Name" };
            vm1.ID = 1;
            vm1.Age = 10;

            await Task.Delay(5000);
            var vm2 = OpenView<TestViewModel>("TestView1");
            vm2.Name = new Name { firstName = "test", lastName = "2Name" };
            vm2.ID = 2;
            vm2.Age = 20;

            await Task.Delay(5000);
            var vm3 = OpenView<TestViewModel1>("TestView");
            vm3.Name = new Name { firstName = "test", lastName = "3Name" };
            vm3.ID = 3;
            vm3.Age = 30;
        }

        TViewModel OpenView<TViewModel>(string viewName) where TViewModel : ViewModel
        {
            var vm = Activator.CreateInstance<TViewModel>();
            var assetLoader = new TestAssetLoader();
            var assetPath = viewName;
            //TODO 这个地方存在字符串拼接   这个可以通过注入解决
            var viewTypeName = $"FUI.Test.{viewName}_{vm.GetType().Name}_Generated";
            var viewType = Type.GetType(viewTypeName);
            var view = Activator.CreateInstance(viewType, vm, assetLoader, assetPath);
            vm.Initialize();
            return vm;
        }
    }
}
