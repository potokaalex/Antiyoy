using System.Collections.Generic;
using ClientCode.Data.Scene;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers
{
    public class MainMenuWindowsHandler : WindowsHandlerBase
    {
        private readonly MainMenuModel _model;
        private Dictionary<WindowType, WindowBase> _windows;

        public MainMenuWindowsHandler(UIFactory factory, MainMenuSceneData sceneData, MainMenuModel model) : base(factory, sceneData.UIRoot) =>
            _model = model;

        private protected override void OnCreate(WindowBase window)
        {
            if (window.Type == WindowType.MapEditorPreload)
            {
                var w = (MapEditorPreloadWindow)window;

                foreach (var key in _model.MapKeys)
                    w.CreateButton(key);

                _model.MapKeys.OnAdded += w.CreateButton;
                _model.MapKeys.OnRemoved += w.RemoveButton;
            }
            
            base.OnCreate(window);
        }

        private protected override void OnDestroy(WindowBase window)
        {
            if (window.Type == WindowType.MapEditorPreload)
            {
                var w = (MapEditorPreloadWindow)window;
                _model.MapKeys.OnAdded -= w.CreateButton;
                _model.MapKeys.OnRemoved -= w.RemoveButton;
            }
        }
    }
}