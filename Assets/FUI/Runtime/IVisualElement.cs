namespace FUI
{
    public interface IVisualElement<TValue>
    {
        void SetValue(TValue value);
    }

    public interface IVisualElement
    {
        void SetValue(object value);
    }
}
