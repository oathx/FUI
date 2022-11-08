using System;

using UnityEngine;

namespace FUI.Test
{
    public abstract class UGUIValueConverter : MonoBehaviour, IValueConverter
    {
        public abstract void Convert(object value);
    }

    public class TestViewGenerated : View
    {
        GameObject gameObject;
        ViewModel bindingContext;

        public void Init()
        {
            var convertes = gameObject.transform.GetComponentsInChildren<UGUIValueConverter>(true);
            foreach(var converterComponent in convertes)
            {
                var converterName = converterComponent.gameObject.name;
                var converter = converterComponent as IValueConverter;
                ulong converterId = ((ulong)converterName.GetHashCode()) << 32 | (uint)converter.GetType().GetHashCode();
                AddConverter(converterId, converter);
            }

            //bindingContext.PropertyChanged += this.Convert;
        }
    }
}