using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUI.Bindable;

namespace FUI
{
    public class View
    {
        internal Dictionary<ulong, IValueConverter> convertes;

        internal void Convert(object sender, string propertyName)
        {
            OnValueChanged<object>(111, sender.GetType().GetProperty(propertyName).GetValue(sender));
        }

        internal void OnValueChanged<T>(ulong converterId, T value)
        {
            if (!convertes.TryGetValue(converterId, out var converter))
            {
                return;
            }

            (converter as IValueConverter<T>).Convert(value);
        }
    }
}
