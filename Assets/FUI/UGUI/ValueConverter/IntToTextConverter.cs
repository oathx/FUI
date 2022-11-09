
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.ValueConverter
{
    [RequireComponent(typeof(Text))]
    public class IntToTextConverter : UGUIValueConverter<int>
    {
        Text text;

        void Awake()
        {
            text = transform.GetComponent<Text>();
        }

        public override void Convert(int value)
        {
            text.text = value.ToString();
        }
    }
}
