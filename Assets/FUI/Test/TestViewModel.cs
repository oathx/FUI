
using System;

namespace FUI.Test
{
    [Binding("SampleView")]
    public class SampleViewModel : ViewModel
    {
        [Binding]
        public string Name { get; set; }

        [Binding]
        public int ID { get; set; }

        [Binding]
        public int Age { get; set; }

        [Binding]
        Action Submit { get; set; }

        public override void Initialize()
        {
            Name = "Test";
            ID = 0;
            Age = 0;
            Submit = OnSubmit;
        }

        void OnSubmit()
        {
            UnityEngine.Debug.Log("ClickBtn....");
        }
    }
}
