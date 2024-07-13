using ClientCode.Gameplay.Ecs;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorExitState : IState
    {
        private readonly EcsFactory _ecsFactory;
        private readonly IStateMachine _stateMachine;

        public MapEditorExitState(EcsFactory ecsFactory, IStateMachine stateMachine)
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