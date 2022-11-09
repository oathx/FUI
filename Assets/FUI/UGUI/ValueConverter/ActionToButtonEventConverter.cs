
using UnityEngine;
using UnityEngine.UI;
using System;

namespace FUI.UGUI.ValueConverter
{
    [RequireComponent(typeof(Button))]
    public class ActionToButtonEventConverter : UGUIValueConverter<Action>
    {
        Button button;

        void Awake()
        {
            button = transform.GetComponent<Button>();
        }

        public override void Convert(Action value)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>value.Invoke());
        }
    }
}
