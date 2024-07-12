using Zenject;

namespace ClientCode.Services.StateMachine
{
    public class StateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container) => _container = container;

        public T Create<T>() where T : IState
        {
            var state = _container.Instantiate<T>();
            _container.Inject(state);
            return state;
        }
    }
}