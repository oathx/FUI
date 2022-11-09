
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.ValueConverter
{
    [RequireComponent(typeof(Text))]
    public class StringToTextConverter : UGUIValueConverter<string>
    {
        Text text;

        void Awake()
        {
            text = transform.GetComponent<Text>();
        }

        public override void Convert(string value)
        {
            text.text = value;
        }
    }
}
