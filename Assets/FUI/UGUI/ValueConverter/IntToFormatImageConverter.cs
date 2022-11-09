
using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.ValueConverter
{
    [RequireComponent(typeof(Image))]
    public class IntToFormatImageConverter : UGUIValueConverter<int>
    {
        Image image;
        public string formatString;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        public override void Convert(int value)
        {
            //暂时这个地方这样加载资源  后续注入资源加载器
            image.sprite = Resources.Load<Sprite>(string.Format(formatString, value));
        }
    }
}
