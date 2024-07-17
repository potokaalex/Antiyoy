using Zenject;

namespace ClientCode.Services.StateMachine
{
    public class StateStartuper<T> : IInitializable where T : IState
    {
        private readonly IStateMachine _stateMachine;

        public StateStartuper(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Initialize() => _stateMachine.SwitchTo<T>();
    }
}