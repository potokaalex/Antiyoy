namespace Code.Services.StateMachine
{
    public class StateMachine : IStateMachine
    {
        private readonly StateFactory _factory;
        private IState _currentState;

        public StateMachine(StateFactory factory) => _factory = factory;

        public void SwitchTo<T>() where T : IState
        {
            _currentState?.Exit();
            _currentState = _factory.Create<T>();
            _currentState.Enter();
        }
    }
}