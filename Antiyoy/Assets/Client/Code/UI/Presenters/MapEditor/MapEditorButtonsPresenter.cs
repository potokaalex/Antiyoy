using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.SaveLoad;
using Zenject;

namespace ClientCode.UI.Presenters.MapEditor
{
    public class MapEditorButtonsPresenter : IButtonsHandler
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Handle(ButtonBase button)
        {
            if (button.GetBaseType() == ButtonType.Load) 
                HandleLoadButton((LoadButton)button);
            else if (button.GetBaseType() == ButtonType.MapSaveLoad)
                HandleMapSaveLoadButton((MapSaveLoadButton)button);
                
        }

        private void HandleLoadButton(LoadButton button)
        {
            if (button.Type == LoadButtonType.MainMenu)
                _stateMachine.SwitchTo<MapEditorExitState>();
        }

        private void HandleMapSaveLoadButton(MapSaveLoadButton button)
        {
            if (button.Type == MapSaveLoadButtonType.Save)
                _stateMachine.SwitchTo<MapEditorSaveState>();
        }
    }
}