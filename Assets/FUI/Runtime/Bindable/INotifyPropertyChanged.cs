using System;

namespace FUI.Bindable
{
    public interface INotifyPropertyChanged
    {
        Action<object, string> PropertyChanged { get; set; }
    }
}
