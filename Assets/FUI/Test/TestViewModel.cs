using System;
using System.Collections.Generic;
using System.Text;

namespace FUI.Test
{
    [Binding("TestView")]
    public class TestViewModel : ViewModel
    {
        [Binding("testText", typeof(StringToTextAdpator))]
        BindableProperty<string> name = new BindableProperty<string>("aaa");

        public void Init()
        {
            name.Value = "bbb";
        }
    }
}
