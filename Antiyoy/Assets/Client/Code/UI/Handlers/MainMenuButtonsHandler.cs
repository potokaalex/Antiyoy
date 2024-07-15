using ClientCode.Data.Progress;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.SaveLoader.Base;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.SaveLoader.Progress.Actors;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.Select;

namespace ClientCode.UI.Handlers
{
    public class MainMenuButtonsHandler : ILoadButtonHandler, IMapSelectButtonHandler, IProgressWriter
    {
        private readonly IStateMachine _stateMachine;
        private readonly IProgressDataSaveLoader _saveLoader;
        private string _selectedMapKey;

        public MainMenuButtonsHandler(IStateMachine stateMachine, IProgressDataSaveLoader saveLoader)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
        }

        void ILoadButtonHandler.Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MapEditor)
                _stateMachine.SwitchTo<MapEditorLoadState>();
        }

        void IMapSelectButtonHandler.Handle(string mapKey)
        {
            _selectedMapKey = mapKey;
            _stateMachine.SwitchTo<MapEditorLoadState>();
        }

        public void OnSave(PlayerProgressData progress) => progress.Map = _saveLoader.LoadMap(_selectedMapKey);
    }
}