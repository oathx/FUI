namespace FUI
{
    public interface IValueConverter<T>
    {
        void Convert(T value);
    }

    public interface IValueConverter
    {
        void Convert(object value);
    }
}
