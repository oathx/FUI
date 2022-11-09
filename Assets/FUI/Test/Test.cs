using System;
using System.Threading.Tasks;

using UnityEngine;

namespace FUI.Test
{
    public class Test : MonoBehaviour
    {
        void Awake()
        {
            OpenView<TestViewGenerated>();
        }

        async void OpenView<T>() where T : View
        {
            var vm = new SampleViewModel();
            var view = Activator.CreateInstance(typeof(T), vm);

            vm.Initialize();

            vm.Name = "test1Name";
            vm.ID = 1;
            vm.Age = 10;

            await Task.Delay(1000);
            vm.Name = "test2Naame";
            vm.ID = 2;
            vm.Age = 20;

            await Task.Delay(1000);
            vm.Name = "test3Name";
            vm.ID = 3;
            vm.Age = 30;
        }
    }
}
