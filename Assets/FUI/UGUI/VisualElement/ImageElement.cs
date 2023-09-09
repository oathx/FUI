﻿using UnityEngine;
using UnityEngine.UI;

namespace FUI.UGUI.VisualElement
{
    [RequireComponent(typeof(Image))]
    public class ImageElement : UGUIVisualElement<Sprite>
    {
        Image image;
        void Awake()
        {
            image = GetComponent<Image>();
        }
        public override void SetValue(Sprite value)
        {
            image.sprite = value;
        }
    }
}
