
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.ValueConverter
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
            //暂时这个地方这样加载资源  后续注入资源加载器
            image.sprite = Resources.Load<Sprite>(string.Format(formatString, value));
        }
    }
}
