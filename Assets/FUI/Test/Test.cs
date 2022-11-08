using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FUI.Test
{
    public class Test : MonoBehaviour
    {
        void Awake()
        {
            var vm = new SampleViewModel();
            vm.PropertyChanged += OnPropertyChanged;
            vm.Name = "testName";
            vm.Initialize();
            vm.Name = "testName2";
        }

        void OnPropertyChanged(object sender, string propertyName)
        {
            UnityEngine.Debug.Log($"OnPropertyChanged  propertyName:{propertyName}  sender:{sender}");
        }
    }
}
