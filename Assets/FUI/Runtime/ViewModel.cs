using FUI.Bindable;

namespace FUI
{
    public class ViewModel : ObservableObject
    {
        public virtual void Initialize() { }

        public virtual void OnOpen(object param) { }

        public virtual void OnFocus() { }

        public virtual void OnUnfocus() { }

        public virtual void OnClose() { }
    }
}
