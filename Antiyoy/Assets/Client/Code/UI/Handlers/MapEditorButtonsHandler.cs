using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.SaveLoad;
using Zenject;

namespace ClientCode.UI.Handlers
{
    public class MapEditorButtonsHandler : ILoadButtonHandler, IMapSaveLoadButtonHandler
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MainMenu)
                _stateMachine.SwitchTo<MapEditorExitState>();
        }

        public void Handle(MapSaveLoadButtonType type)
        {
            if (type == MapSaveLoadButtonType.Save)
                _stateMachine.SwitchTo<MapEditorSaveState>();
        }
    }
}