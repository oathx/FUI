
using System;

namespace FUI.Test
{
    [Binding("TestView")]
    public class TestViewModel1 : TestViewModel
    {
        public override void Initialize()
        {
            Name = new Name { firstName = "TestViewModel", lastName = "1" };
            ID = 0;
            Age = 0;
            Submit = OnSubmit;
        }

        protected override void OnSubmit()
        {
            UnityEngine.Debug.Log($"ClickBtn  TestViewModel1...");
        }
    }
}
