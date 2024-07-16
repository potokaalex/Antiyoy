using ClientCode.Services.CanvasService;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers
{
    public class ProjectWindowsHandler : IWindowsHandler
    {
        private readonly ProjectCanvasController _canvasController;

        public ProjectWindowsHandler(ProjectCanvasController canvasController) => _canvasController = canvasController;

        public void OnCreate(WindowBase window)
        {
            if (window.Type == WindowType.Popups)
                window.transform.SetParent(_canvasController.TopElementsRoot, false);
        }
    }
}