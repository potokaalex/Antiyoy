using ClientCode.Services.StateMachine;
using Zenject;

namespace ClientCode.Services.Startup
{
    public class AutoStartupper<T> : IInitializable where T : IState
    {
        private readonly IStateMachine _stateMachine;

        public AutoStartupper(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Initialize() => _stateMachine.SwitchTo<T>();
    }
}