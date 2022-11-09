
using FUI.UGUI.ValueConverter;

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

        public override void Initialize()
        {
            Name = "aaaaa";
        }

        public string GetName()
        {
            return Name;
        }
    }
}
