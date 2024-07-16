using System.Collections.Generic;
using ClientCode.UI.Models;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers.MainMenu
{
    public class MainMenuWindowsHandler : IWindowsHandler
    {
        private readonly MainMenuModel _model;
        private Dictionary<WindowType, WindowBase> _windows;

        public MainMenuWindowsHandler(MainMenuModel model) => _model = model;

        public void OnCreate(WindowBase window)
        {
            if (window.Type == WindowType.MapEditorPreload)
            {
                var w = (MapEditorPreloadWindow)window;

                foreach (var key in _model.MapKeys)
                    w.CreateButton(key);

                _model.MapKeys.OnAdded += w.CreateButton;
                _model.MapKeys.OnRemoved += w.RemoveButton;
            }
        }

        public  void OnDestroy(WindowBase window)
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