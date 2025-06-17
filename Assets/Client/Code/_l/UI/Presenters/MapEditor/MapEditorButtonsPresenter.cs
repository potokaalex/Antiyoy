using ClientCode.Client.Code.Services.StateMachineCode;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.SaveLoad;
using ClientCode.UI.Buttons.MapEditor;
using ClientCode.UI.Models;

namespace ClientCode.UI.Presenters.MapEditor
{
    public class MapEditorButtonsPresenter : IButtonsHandler
    {
        private readonly StateMachine _stateMachine;
        private readonly MapEditorModel _model;

        public MapEditorButtonsPresenter(MapEditorModel model, StateMachine stateMachine)
        {
            _model = model;
            _stateMachine = stateMachine;
        }

        public void Handle(ButtonBase button)
        {
            var baseType = button.GetBaseType();

            if (baseType == ButtonType.Load)
                HandleLoadButton((LoadButton)button);
            else if (baseType == ButtonType.MapSaveLoad)
                HandleMapSaveLoadButton((MapSaveLoadButton)button);
            else if (baseType == ButtonType.MapEditorMode)
                HandleMapEditorModeButton((MapEditorModeButton)button);
        }

        private void HandleMapEditorModeButton(MapEditorModeButton button) => _model.ModeType = button.Type;

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