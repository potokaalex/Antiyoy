using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.SaveLoad;
using ClientCode.UI.Buttons.Map.Select;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers
{
    public class MainMenuButtonsHandler : ILoadButtonHandler, IMapSelectButtonHandler, IMapSaveLoadButtonHandler
    {
        private readonly IStateMachine _stateMachine;
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IWindowsHandler _windowsHandler;
        private readonly MainMenuModel _model;

        public MainMenuButtonsHandler(IStateMachine stateMachine, IProgressDataSaveLoader saveLoader, ILogReceiver logReceiver,
            IStaticDataProvider staticDataProvider, IWindowsHandler windowsHandler, MainMenuModel model)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _staticDataProvider = staticDataProvider;
            _windowsHandler = windowsHandler;
            _model = model;
        }

        void ILoadButtonHandler.Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MapEditor)
            {
                if (_model.MapKeys.Count >= _staticDataProvider.Configs.Progress.MaxMapsSavesCount)
                    _logReceiver.Log(new LogData(LogType.Error, "Map create error: Reached the maximum maps cout, please remove one!"));
                else
                    _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }

        void IMapSelectButtonHandler.Handle(string mapKey)
        {
            var validatorResult = _saveLoader.IsMapValidToLoad(mapKey);

            if (validatorResult == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Map select error: file is not exits!"));
            else if (validatorResult == SaveLoaderResultType.ErrorFileIsDamaged)
                _logReceiver.Log(new LogData(LogType.Error, "Map select error: file is damaged!"));
            else if (validatorResult == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Map select error: unknown reason!"));
            else
            {
                _model.SelectedMapKey = mapKey;
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }

        async void IMapSaveLoadButtonHandler.Handle(MapSaveLoadButtonType type)
        {
            if (type == MapSaveLoadButtonType.Remove)
            {
                var writingWindow = (WritingWindow)_windowsHandler.Get(WindowType.Writing);
                writingWindow.Open();

                var mapKey = await writingWindow.GetString();
                var result = _saveLoader.RemoveMap(mapKey);

                if (result == SaveLoaderResultType.ErrorFileIsNotExist)
                    _logReceiver.Log(new LogData(LogType.Error, "Map remove error: map doesnt exist!"));
                else if (result == SaveLoaderResultType.Error)
                    _logReceiver.Log(new LogData(LogType.Error, "Map remove error: unknown reason!"));
                else
                    _model.MapKeys.Remove(mapKey);

                writingWindow.Close();
            }
        }
    }
}