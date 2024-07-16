using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Map;
using ClientCode.UI.Models;

namespace ClientCode.UI.Presenters.MainMenu
{
    public class MainMenuMapSelectButtonsPresenter : IButtonsHandler
    {
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly MainMenuModel _model;
        private readonly IStateMachine _stateMachine;

        public MainMenuMapSelectButtonsPresenter(IProgressDataSaveLoader saveLoader, ILogReceiver logReceiver, MainMenuModel model,
            IStateMachine stateMachine)
        {
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _model = model;
            _stateMachine = stateMachine;
        }

        public void Handle(ButtonBase button) => HandleMapSelectButton((MapSelectButton)button);

        private void HandleMapSelectButton(MapSelectButton button)
        {
            var mapKey = button.MapKey;
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
    }
}