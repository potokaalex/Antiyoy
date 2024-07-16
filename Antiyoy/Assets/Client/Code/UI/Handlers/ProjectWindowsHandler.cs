using ClientCode.Services.CanvasService;
using ClientCode.UI.Windows.Base;
using Zenject;

namespace ClientCode.UI.Handlers
{
    public class ProjectWindowsHandler : WindowsHandlerBase, IInitializable
    {
        private readonly ProjectCanvasController _canvasController;

        public ProjectWindowsHandler(UIFactory factory, ProjectCanvasController canvasController) : base(factory, null) =>
            _canvasController = canvasController;

        public void Initialize() => WindowsRoot = _canvasController.DefaultElementsRoot;

        public override void OnBeforeOpen(WindowBase window)
        {
            if (window.Type == WindowType.Popups)
                window.transform.SetParent(_canvasController.TopElementsRoot, false);
        }
    }
}