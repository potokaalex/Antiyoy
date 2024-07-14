using ClientCode.Data.Scene;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers
{
    public class MainMenuWindowsHandler : WindowsHandler
    {
        private readonly IProgressDataProvider _progressDataProvider;

        public MainMenuWindowsHandler(UIFactory factory, MainMenuSceneData sceneData, IProgressDataProvider progressDataProvider)
            : base(factory, sceneData.UIRoot)
        {
            _progressDataProvider = progressDataProvider;
        }

        private protected override void Open(WindowBase window)
        {
            if (window is MapEditorPreloadWindow preloadWindow)
                preloadWindow.Initialize(_progressDataProvider.Project.MapKeys);

            base.Open(window);
        }
    }
}