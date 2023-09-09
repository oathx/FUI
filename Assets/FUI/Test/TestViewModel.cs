
using FUI.Bindable;

using System;
using System.Collections.Generic;

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
    public partial class TestViewModel : ViewModel
    {
        public Name _name;
        [Binding]
        public Name Name
        {
            get
            {
                return _name;
            }
            set
            {
                if(EqualityComparer<Name>.Default.Equals(_name, value))
                {
                    return;
                }
                this._Name_Changed?.Invoke(this, _name, value);
                _name = value;
            }
        }

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
