using ClientCode.Infrastructure.States.Project;
using ClientCode.Services.StateMachine;
using Zenject;

namespace ClientCode.Infrastructure.Startup
{
    public class Bootstrap : IInitializable
    {
        private readonly IStateMachine _stateMachine;

        public Bootstrap(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Initialize() => _stateMachine.SwitchTo<ProjectLoadState>();
    }
}