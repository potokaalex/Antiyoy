using ClientCode.Gameplay.Countries.Components;
using ClientCode.Gameplay.Ecs;
using Leopotam.EcsLite;

namespace ClientCode.Gameplay.Countries.Systems
{
    public class CountryRemoveRegionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly CountryFactory _factory;
        private EcsFilter _requestFilter;
        private EcsPool<CountryRemoveRegionRequest> _requestPool;
        private EcsPool<CountryComponent> _pool;
        private EcsPool<CountryLink> _linkPool;

        public CountryRemoveRegionSystem(IEcsProvider ecsProvider, CountryFactory factory)
        {
            _ecsProvider = ecsProvider;
            _factory = factory;
        }

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();
            var eventsBus = _ecsProvider.GetEventsBus();

            _requestFilter = eventsBus.GetEventBodies(out _requestPool);
            _pool = world.GetPool<CountryComponent>();
            _linkPool = world.GetPool<CountryLink>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter)
                Remove(_requestPool.Get(entity));
        }

        private void Remove(CountryRemoveRegionRequest request)
        {
            //почему regionEntity удалён, на нём же должна висеть ссылка!
            //баг связан с удалением региона ? Удаление региона вызывается дважды ? (для простоты отладки лучше всего будет исполнять ecs в update)
            var link = _linkPool.Get(request.RegionEntity); 
            
            var country = _pool.Get(link.CountryEntity);
            country.RegionsEntities.Remove(request.RegionEntity);

            if (country.RegionsEntities.Count == 0)
                _factory.Destroy(request.RegionEntity);
        }
    }
}