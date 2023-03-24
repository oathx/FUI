namespace FUI
{
    public interface IVisualElement<TValue>
    {
        void OnValueChanged(TValue value);
    }

    public interface IVisualElement
    {
        void OnValueChanged(object value);
    }
}
