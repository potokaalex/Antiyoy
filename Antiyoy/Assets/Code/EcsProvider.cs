using Leopotam.EcsLite;

namespace Code
{
    public class EcsProvider
    {
        private readonly EcsFactory _factory;
        private EcsWorld _world;

        public EcsProvider(EcsFactory factory) => _factory = factory;

        public void Initialize() => _world = _factory.CreateWorld();

        public EcsWorld GetWorld() => _world;
    }
}