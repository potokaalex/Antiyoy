using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using Zenject;

namespace ClientCode.Infrastructure.Startup
{
    public class MapEditorStartup : IInitializable
    {
        private readonly IStateMachine _stateMachine;

        public MapEditorStartup(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Initialize() => _stateMachine.SwitchTo<MapEditorEnterState>();
    }
}