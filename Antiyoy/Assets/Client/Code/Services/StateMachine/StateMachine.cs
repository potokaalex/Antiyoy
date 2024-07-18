using System;

namespace ClientCode.Services.StateMachine
{
    public class StateMachine : IStateMachine
    {
        private readonly StateFactory _factory;
        private IState _currentState;

        public StateMachine(StateFactory factory) => _factory = factory;

        public void SwitchTo<T>() where T : IState => SwitchTo(typeof(T));

        private void SwitchTo(Type type)
        {
            _currentState?.Exit();
            _currentState = _factory.Create(type);
            _currentState.Enter();
        }
    }
}