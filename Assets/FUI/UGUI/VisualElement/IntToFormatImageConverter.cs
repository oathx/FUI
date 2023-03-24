
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.VisualElement
{
    [RequireComponent(typeof(Image))]
    public class IntToFormatImageConverter : UGUIVisualElement<int>
    {
        Image image;
        public string formatString;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        public override void OnValueChanged(int value)
        {
            image.sprite = AssetLoader.Load<Sprite>(string.Format(formatString, value));
        }
    }
}
