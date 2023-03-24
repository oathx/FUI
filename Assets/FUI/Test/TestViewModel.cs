
using System;

namespace FUI.Test
{
    public class Name
    {
        public string firstName;
        public string lastName;

        public override string ToString()
        {
            return $"{lastName} {firstName}";
        }
    } 

    [Binding("TestView")]
    [Binding("TestView1")]
    public class TestViewModel : ViewModel
    {
        [Binding]
        public Name Name { get; set; }

        [Binding]
        public int ID { get; set; }

        [Binding]
        public int Age { get; set; }

        [Binding]
        public Action Submit { get; set; }

        public override void Initialize()
        {
            Name = new Name { firstName = "Test", lastName = "1" };
            ID = 0;
            Age = 0;
            Submit = OnSubmit;
        }

        protected virtual void OnSubmit()
        {
            UnityEngine.Debug.Log("ClickBtn TestViewModel....");
        }
    }
}
