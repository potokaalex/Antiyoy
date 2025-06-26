using Client.Code.Services;
using ClientCode.Gameplay.Region.Systems;
using Leopotam.EcsLite;
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
            systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(entityNameFormat: "D4"));
            systems.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
            systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("Events", true, "D4"));
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