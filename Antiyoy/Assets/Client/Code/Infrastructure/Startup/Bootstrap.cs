using ClientCode.Infrastructure.States;
using ClientCode.Services.StateMachine;
using ClientCode.Services.Updater;
using Zenject;

namespace ClientCode.Infrastructure.Startup
{
    public class Bootstrap : IInitializable
    {
        private readonly IStateMachine _stateMachine;
        private readonly IUpdater _updater;

        public Bootstrap(IStateMachine stateMachine, IUpdater updater)
        {
            _stateMachine = stateMachine;
            _updater = updater;
        }

        public void Initialize()
        {
            _updater.OnProjectExit += () => _stateMachine.SwitchTo<ProjectExitState>();
            _stateMachine.SwitchTo<ProjectLoadState>();
        }
    }
}