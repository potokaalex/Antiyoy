using System;
using Zenject;

namespace ClientCode.Services.StateMachine
{
    public class StateFactory
    {
        private readonly IInstantiator _container;

        public StateFactory(IInstantiator container) => _container = container;

        public IState Create(Type stateType) => (IState)_container.Instantiate(stateType);
    }
}