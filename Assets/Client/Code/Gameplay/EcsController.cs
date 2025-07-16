using Client.Code.Core;
using Client.Code.Gameplay.Region;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using SevenBoldPencil.EasyEvents;
using Zenject;

namespace Client.Code.Gameplay
{
    public class EcsController : IInitializable, ITickable
    {
        private readonly Instantiator _instantiator;
        private IEcsSystems _systems;

        public EcsController(Instantiator instantiator) => _instantiator = instantiator;

        public EcsWorld World { get; private set; }

        public EventsBus EventBus { get; private set; }

        public void Initialize()
        {
            World = new EcsWorld();
            EventBus = new EventsBus();
            _systems = CreateSystems();
            _systems.Init();
        }

        public void Tick() => _systems.Run();

        private IEcsSystems CreateSystems()
        {
            var systems = new EcsSystems(World);
            systems.AddWorld(EventBus.GetEventsWorld(), "Events");

            AddStandardDebugSystems(systems);
            AddRegionDebugSystem(systems);
            return systems;
        }

        private void AddStandardDebugSystems(IEcsSystems systems)
        {
#if UNITY_EDITOR
            systems.Add(new EcsWorldDebugSystem(entityNameFormat: "D4"));
            systems.Add(new EcsSystemsDebugSystem());
            systems.Add(new EcsWorldDebugSystem("Events", true, "D4"));
#endif
        }

        private void AddRegionDebugSystem(EcsSystems systems)
        {
#if DEBUG_PROJECT
            systems.Add(_instantiator.Instantiate<RegionDebugSystem>());
#endif
        }
    }
}