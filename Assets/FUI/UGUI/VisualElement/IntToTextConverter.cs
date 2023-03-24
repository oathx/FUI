
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.VisualElement
{
    [RequireComponent(typeof(Text))]
    public class IntToTextConverter : UGUIVisualElement<int>
    {
        Text text;

        void Awake()
        {
            text = transform.GetComponent<Text>();
        }

        public override void OnValueChanged(int value)
        {
            text.text = value.ToString();
        }
    }
}
