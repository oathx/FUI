
using FUI.Bindable;

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

    public class ItemData : ObservableObject
    {
        [Binding]
        public int ID { get; set; }
        [Binding]
        public string Name { get; set; }
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

        [Binding]
        public ObservableList<ItemData> List { get; set; }

        //public override void Initialize()
        //{
        //    Name = new Name { firstName = "Test", lastName = "1" };
        //    ID = 0;
        //    Age = 0;
        //    Submit = OnSubmit;
        //    List = new ObservableList<ItemData> { 1, 2, 3 };
        //}

        protected virtual void OnSubmit()
        {
            UnityEngine.Debug.Log("ClickBtn TestViewModel....");
        }
    }
}
