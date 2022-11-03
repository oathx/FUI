namespace FUI
{
    public interface IAdpator<T>
    {
        void OnValueChanged(T oldValue, T newValue);
    }

    public interface IAdpator
    {
        void OnValueChanged(object oldValue, object newValue);
    }
}
