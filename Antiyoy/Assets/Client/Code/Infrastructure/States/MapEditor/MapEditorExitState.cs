using ClientCode.Gameplay.Ecs;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorExitState : IState
    {
        private readonly EcsFactory _ecsFactory;

        public MapEditorExitState(EcsFactory ecsFactory) => _ecsFactory = ecsFactory;

        public void Enter() => _ecsFactory.Destroy();
    }
}