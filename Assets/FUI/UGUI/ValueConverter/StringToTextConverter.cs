
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.ValueConverter
{
    [RequireComponent(typeof(Text))]
    public class StringToTextConverter : UGUIVisualElement<string>
    {
        Text text;

        void Awake()
        {
            text = transform.GetComponent<Text>();
        }

        public override void OnValueChanged(string value)
        {
            text.text = value;
        }
    }
}
