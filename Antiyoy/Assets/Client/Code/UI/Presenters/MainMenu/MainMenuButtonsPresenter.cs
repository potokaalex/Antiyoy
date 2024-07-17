using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.SaveLoad;
using ClientCode.UI.Factory;
using ClientCode.UI.Models;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Presenters.MainMenu
{
    public class MainMenuButtonsPresenter : IButtonsHandler
    {
        private readonly IStateMachine _stateMachine;
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly WindowsFactory _windowsFactory;
        private readonly MainMenuModel _model;

        public MainMenuButtonsPresenter(IStateMachine stateMachine, IMapSaveLoader saveLoader, ILogReceiver logReceiver,
            IStaticDataProvider staticDataProvider, WindowsFactory windowsFactory, MainMenuModel model)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _staticDataProvider = staticDataProvider;
            _windowsFactory = windowsFactory;
            _model = model;
        }

        public void Handle(ButtonBase button)
        {
            if (button.GetBaseType() == ButtonType.Load)
                HandleLoadButton((LoadButton)button);
            else if (button.GetBaseType() == ButtonType.MapSaveLoad)
                HandleMapSaveLoadButton((MapSaveLoadButton)button);
        }

        private async void HandleMapSaveLoadButton(MapSaveLoadButton button)
        {
            if (button.Type != MapSaveLoadButtonType.Remove)
                return;

            var writingWindow = (WritingWindow)_windowsFactory.Get(WindowType.Writing);
            writingWindow.Open();

            var mapKey = await writingWindow.GetString();
            var result = _saveLoader.Remove(mapKey);

            if (result == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Map remove error: map doesnt exist!"));
            else if (result == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Map remove error: unknown reason!"));
            else
                _model.MapKeys.Remove(mapKey);

            writingWindow.Close();
        }

        private void HandleLoadButton(LoadButton button)
        {
            if (button.Type == LoadButtonType.MapEditor)
            {
                if (_model.MapKeys.Count >= _staticDataProvider.Configs.Progress.MaxMapsSavesCount)
                    _logReceiver.Log(new LogData(LogType.Error, "Map create error: Reached the maximum maps cout, please remove one!"));
                else
                {
                    _model.SelectedMapKey = null;
                    _stateMachine.SwitchTo<MapEditorLoadState>();
                }
            }
        }
    }
}