using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Map;
using ClientCode.UI.Models;

namespace ClientCode.UI.Presenters.MainMenu
{
    public class MainMenuMapSelectButtonsPresenter : IButtonsHandler
    {
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly MainMenuModel _model;
        private readonly IProjectStateMachine _stateMachine;

        public MainMenuMapSelectButtonsPresenter(IMapSaveLoader saveLoader, ILogReceiver logReceiver, MainMenuModel model,
            IProjectStateMachine stateMachine)
        {
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _model = model;
            _stateMachine = stateMachine;
        }

        public void Handle(ButtonBaseOld button)
        {
            //HandleMapSelectButton((MapEditorSelectButton)button);
        }

        private void HandleMapSelectButton()
        {
            /*
            var mapKey = button.MapKey;
            var validatorResult = _saveLoader.IsValidToLoad(mapKey);

            if (validatorResult == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Map select error: file is not exits!"));
            else if (validatorResult == SaveLoaderResultType.ErrorFileIsDamaged)
                _logReceiver.Log(new LogData(LogType.Error, "Map select error: file is damaged!"));
            else if (validatorResult == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Map select error: unknown reason!"));
            else
            {
                _model.MapEditorPreload.MapKey = mapKey;
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
            */
        }
    }
}