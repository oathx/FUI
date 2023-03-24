using System.Collections.Specialized;

namespace FUI.Bindable
{
    public delegate void PropertyChangedHandler(object sender, string propertyName);
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
