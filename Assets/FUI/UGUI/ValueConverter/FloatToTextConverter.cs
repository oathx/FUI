
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.ValueConverter
{
    [RequireComponent(typeof(Text))]
    public class FloatToTextConverter : UGUIValueConverter<float>
    {
        Text text;

        void Awake()
        {
            text = transform.GetComponent<Text>();
        }

        public override void Convert(float value)
        {
            text.text = value.ToString();
        }
    }
}
