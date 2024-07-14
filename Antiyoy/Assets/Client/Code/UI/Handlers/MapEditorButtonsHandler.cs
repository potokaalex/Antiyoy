using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Load;
using Zenject;

namespace ClientCode.UI.Handlers
{
    public class MapEditorButtonsHandler : ILoadButtonHandler
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MainMenu)
                _stateMachine.SwitchTo<MapEditorExitState>();
        }
    }
}