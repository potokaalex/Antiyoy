using System.Collections.Generic;
using ClientCode.UI.Models;
using ClientCode.UI.Windows.Base;
using ClientCode.UI.Windows.Writing;

namespace ClientCode.UI.Presenters.MainMenu
{
    public class MainMenuWindowsPresenter : IWindowsHandler
    {
        private readonly MainMenuModel _model;
        private readonly MainMenuMapSelectButtonsPresenter _mapSelectButtonsPresenter;
        private Dictionary<WindowType, WindowBase> _windows;

        public MainMenuWindowsPresenter(MainMenuModel model, MainMenuMapSelectButtonsPresenter mapSelectButtonsPresenter)
        {
            _model = model;
            _mapSelectButtonsPresenter = mapSelectButtonsPresenter;
        }

        public void OnCreate(WindowBase window)
        {
            if (window.BaseType == WindowType.MapEditorPreload)
            {
                var w = (MapEditorPreloadWindow)window;

                w.Initialize(_mapSelectButtonsPresenter);

                foreach (var key in _model.MapKeys)
                    w.CreateButton(key);

                _model.MapKeys.OnAdded += w.CreateButton;
                _model.MapKeys.OnRemoved += w.RemoveButton;
            }
        }

        public void OnDestroy(WindowBase window)
        {
            if (window.BaseType == WindowType.MapEditorPreload)
            {
                var w = (MapEditorPreloadWindow)window;
                _model.MapKeys.OnAdded -= w.CreateButton;
                _model.MapKeys.OnRemoved -= w.RemoveButton;
            }
        }
    }
}