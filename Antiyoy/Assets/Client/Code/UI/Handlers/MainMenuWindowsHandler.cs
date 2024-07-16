using System.Collections.Generic;
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
        private Dictionary<WindowType, WindowBase> _windows;

        public MainMenuWindowsHandler(UIFactory factory, MainMenuSceneData sceneData) : base(factory, sceneData.UIRoot)
        {
        }

        public override void OnBeforeOpen(WindowBase window)
        {
            if (window.Type == WindowType.MapEditorPreload)
                ((MapEditorPreloadWindow)window).Initialize(_progress.Player.MapKeys);
        }

        public void OnLoad(ProgressData progress) => _progress = progress;
    }
}