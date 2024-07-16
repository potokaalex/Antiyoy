using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.Select;

namespace ClientCode.UI.Handlers
{
    public class MainMenuButtonsHandler : ILoadButtonHandler, IMapSelectButtonHandler, IProgressReader, IProgressWriter
    {
        private readonly IStateMachine _stateMachine;
        private readonly IProgressDataSaveLoader _saveLoader;
        private string _selectedMapKey;
        private readonly ILogReceiver _logReceiver;
        private readonly IStaticDataProvider _staticDataProvider;
        private ProgressData _progress;

        public MainMenuButtonsHandler(IStateMachine stateMachine, IProgressDataSaveLoader saveLoader, ILogReceiver logReceiver,
            IStaticDataProvider staticDataProvider)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _staticDataProvider = staticDataProvider;
        }

        void ILoadButtonHandler.Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MapEditor)
            {
                if (_progress.Player.MapKeys.Length >= _staticDataProvider.Configs.Progress.MaxMapsSavesCount)
                    _logReceiver.Log(new LogData(LogType.Error, "Map error: Reached the maximum maps cout, please remove one!"));
                else
                    _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }

        void IMapSelectButtonHandler.Handle(string mapKey)
        {
            var validatorResult = _saveLoader.IsMapValidToLoad(mapKey);

            if (validatorResult == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Map error: file is not exits!"));
            else if (validatorResult == SaveLoaderResultType.ErrorFileIsDamaged)
                _logReceiver.Log(new LogData(LogType.Error, "Map error: file is damaged!"));
            else if (validatorResult == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Map error: unknown reason!"));
            else
            {
                _selectedMapKey = mapKey;
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }

        public void OnLoad(ProgressData progress) => _progress = progress;

        public Task OnSave(ProgressData progress)
        {
            progress.Player.SelectedMapKey = _selectedMapKey;
            return Task.CompletedTask;
        }
    }
}