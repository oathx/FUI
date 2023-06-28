using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.VisualElement
{
    [RequireComponent(typeof(Text))]
    public class ObjectToText : UGUIVisualElement
    {
        Text text;

        void Awake()
        {
            text = GetComponent<Text>();
        }

        public override void OnValueChanged(object value)
        {
            text.text = value?.ToString();
        }
    }
}
