
using UnityEngine;
using UnityEngine.UI;
using System;

namespace FUI.UGUI.VisualElement
{
    [RequireComponent(typeof(Button))]
    public class ActionToButtonEventConverter : UGUIVisualElement<Action>
    {
        Button button;

        void Awake()
        {
            button = transform.GetComponent<Button>();
        }

        public override void OnValueChanged(Action value)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>value.Invoke());
        }
    }
}
