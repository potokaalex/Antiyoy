using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Data.Scene;
using ClientCode.Services.Progress.Actors;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers
{
    public class MainMenuWindowsHandler : WindowsHandlerBase, IProgressReader
    {
        private ProgressData _progress;

        public MainMenuWindowsHandler(UIFactory factory, MainMenuSceneData sceneData) : base(factory, sceneData.UIRoot)
        {
        }

        public override void OnBeforeOpen(WindowBase window)
        {
            if (window is MapEditorPreloadWindow preloadWindow)
                preloadWindow.Initialize(_progress.Player.MapKeys);
        }

        public Task OnLoad(ProgressData progress)
        {
            _progress = progress;
            return Task.CompletedTask;
        }
    }
}