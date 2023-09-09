namespace FUI.UGUI.VisualElement
{
    public class VisibleElement : UGUIVisualElement<bool>
    {
        public override void SetValue(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
