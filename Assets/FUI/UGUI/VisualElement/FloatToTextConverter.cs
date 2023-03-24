
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.VisualElement
{
    [RequireComponent(typeof(Text))]
    public class FloatToTextConverter : UGUIVisualElement<float>
    {
        Text text;

        void Awake()
        {
            text = transform.GetComponent<Text>();
        }

        public override void OnValueChanged(float value)
        {
            text.text = value.ToString();
        }
    }
}
