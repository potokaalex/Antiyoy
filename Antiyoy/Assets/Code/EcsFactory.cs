using Code.Region;
using Code.Tile;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using SevenBoldPencil.EasyEvents;
using Zenject;

namespace Code
{
    public class EcsFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly EcsProvider _provider;

        private EcsWorld _world;
        private IEcsSystems _systems;
        private EventsBus _eventBus;

        public EcsFactory(IInstantiator instantiator, EcsProvider provider)
        {
            _instantiator = instantiator;
            _provider = provider;
        }
        
        public void Create()
        {
            _world = new EcsWorld();
            _eventBus = new EventsBus();
            _systems = CreateSystems();

            _provider.Initialize(_world, _eventBus, _systems);
        }

        public void Destroy()
        {
            _systems.Destroy();
            _world.Destroy();
            _eventBus.Destroy();
        }

        private IEcsSystems CreateSystems()
        {
            var systems = new EcsSystems(_world);
            systems.AddWorld(_eventBus.GetEventsWorld(), "Events");

            AddDebugSystems(systems);
            systems.Add(CreateSystem<TileCreateSystem>());
            systems.Add(CreateSystem<TileDestroySystem>());
            systems.Add(CreateSystem<RegionJoinSystem>());
            systems.Add(CreateSystem<RegionDivideSystem>());
            systems.Add(CreateSystem<RegionDebugSystem>());
            systems.Add(_eventBus.GetDestroyEventsSystem().IncReplicant<TileCreateRequest>());
            systems.Add(_eventBus.GetDestroyEventsSystem().IncReplicant<TileDestroyRequest>());

            return systems;
        }

        private void AddDebugSystems(IEcsSystems systems)
        {
#if UNITY_EDITOR
            systems.Add(new EcsWorldDebugSystem(entityNameFormat: "D4"));
            systems.Add(new EcsSystemsDebugSystem());
            systems.Add(new EcsWorldDebugSystem("Events", true, "D4"));
#endif
        }

        private IEcsSystem CreateSystem<T>() where T : IEcsSystem => _instantiator.Instantiate<T>();
    }
}