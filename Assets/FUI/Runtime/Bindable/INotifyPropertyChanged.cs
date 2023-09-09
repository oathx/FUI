using System.Collections.Specialized;

namespace FUI.Bindable
{
    public delegate void PropertyChangedHandler(object sender, string propertyName);
    public delegate void PropertyChangedHandler<T>(object sender, T preValue, T newValue);
    public interface INotifyPropertyChanged
    {
        event PropertyChangedHandler PropertyChanged;
    }

    public delegate void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs args);
    public interface INotifyCollectionChanged
    {
        event CollectionChangedHandler CollectionChanged;
    }
}
