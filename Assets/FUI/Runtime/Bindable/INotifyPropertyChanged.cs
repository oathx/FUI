using System;

namespace FUI.Bindable
{
    public delegate void PropertyChangedHandler(object sender, string propertyName);
    public interface INotifyPropertyChanged
    {
        event PropertyChangedHandler PropertyChanged;
    }
}
