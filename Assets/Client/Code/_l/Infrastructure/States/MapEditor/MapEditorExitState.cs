using Client.Code.Services.StateMachineCode.State;
using ClientCode.Gameplay.Ecs;
using ClientCode.Infrastructure.States.MainMenu;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorExitState : IStateSimple
    {
        private readonly EcsFactory _ecsFactory;
        private readonly IProjectStateMachine _stateMachine;

        public MapEditorExitState(EcsFactory ecsFactory, IProjectStateMachine stateMachine)
        {
            _ecsFactory = ecsFactory;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _ecsFactory.Destroy();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }
    }
}