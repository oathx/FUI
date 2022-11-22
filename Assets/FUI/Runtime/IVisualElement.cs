namespace FUI
{
    public interface IVisualElement<T>
    {
        void OnValueChanged(T value);
    }

    public interface IVisualElement
    {
        void OnValueChanged(object value);
    }
}
