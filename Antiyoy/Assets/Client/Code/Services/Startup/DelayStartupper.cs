using ClientCode.Services.StateMachine;
using Zenject;

namespace ClientCode.Services.Startup
{
    public class DelayStartupper<T> : IInitializable where T : IState
    {
        private readonly IInstantiator _instantiator;

        public DelayStartupper(IInstantiator instantiator) => _instantiator = instantiator;

        public void Initialize() => _instantiator.InstantiateComponentOnNewGameObject<Startupper>(new[] { typeof(T) });
    }
}