using FUI.Bindable;

using System.Collections.Generic;
using System.Reflection;

namespace FUI
{
    public abstract class ViewModel : ObservableObject
    {
        Dictionary<string, PropertyInfo> propertyCache;

        public T GetProperty<T>(string propertyName)
        {
            if(propertyCache == null)
            {
                propertyCache= new Dictionary<string, PropertyInfo>(this.GetType().GetProperties().Length);
            }

            if(!propertyCache.TryGetValue(propertyName, out var property))
            {
                property = this.GetType().GetProperty(propertyName);
                propertyCache.Add(propertyName, property);
            }

            if (property == null)
            {
                return default(T);
            }
            return (T)property.GetValue(this);
        }
    }
}
