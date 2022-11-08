using System.Collections.Generic;

namespace FUI
{
    public class View
    {
        internal Dictionary<ulong, IValueConverter> convertes;

        public void AddConverter(ulong id, IValueConverter converter)
        {
            convertes[id] = converter;
        }

        public void RemoveConverter(ulong id)
        {
            convertes.Remove(id);
        }

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
