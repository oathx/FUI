using UnityEditor;

namespace Unity.UI.Builder
{
    internal class BuilderUssPreviewWindow : BuilderPaneWindow
    {
        BuilderUssPreview m_UssPreview;

        //[MenuItem("Window/UI/UI Builder USS Preview")]
        public static void ShowWindow()
        {
            GetWindowAndInit<BuilderUssPreviewWindow>("UI Builder USS Preview");
        }

        public override void CreateUI()
        {
            var root = rootVisualElement;

            var viewportWindow = document.primaryViewportWindow;
            if (viewportWindow == null)
                return;

            var selection = viewportWindow.selection;

            m_UssPreview = new BuilderUssPreview(this);

            selection.AddNotifier(m_UssPreview);

            root.Add(m_UssPreview);
        }

        public override void ClearUI()
        {
            if (m_UssPreview == null)
                return;

            var selection = document.primaryViewportWindow?.selection;
            if (selection == null)
                return;

            selection.RemoveNotifier(m_UssPreview);

            m_UssPreview.RemoveFromHierarchy();
            m_UssPreview = null;
        }
    }
}