using HSDRawViewer.Rendering.Renderers;

namespace HSDRawViewer.Rendering.Widgets
{
    public interface IWidget
    {
        public bool Interacting { get; }

        public void MouseDown(PickInformation pick);

        public void MouseUp();

        public void Drag(PickInformation pick);

        public void Render(Camera camera, GLTextRenderer text = null);
    }
}
