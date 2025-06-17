using ClientCode.Services.CanvasService;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Presenters
{
    public class ProjectWindowsPresenter : IWindowsHandler
    {
        private readonly ProjectCanvasController _canvasController;

        public ProjectWindowsPresenter(ProjectCanvasController canvasController) => _canvasController = canvasController;

        public void OnCreate(WindowBase window)
        {
            //if (window.BaseType == WindowType.Popups)
            //    window.transform.SetParent(_canvasController.TopElementsRoot, false);
        }
    }
}