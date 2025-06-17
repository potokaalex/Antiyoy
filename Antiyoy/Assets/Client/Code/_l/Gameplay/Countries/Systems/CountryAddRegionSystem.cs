using System.Collections;
using ClientCode.Gameplay.Countries.Components;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;

namespace ClientCode.Gameplay.Countries.Systems
{
    public class CountryAddRegionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly CountryFactory _factory;
        private EcsPool<CountryAddRegionRequest> _requestPool;
        private EcsFilter _requestFilter;
        private EcsPool<CountryComponent> _pool;
        private EcsPool<CountryLink> _linkPool;

        public CountryAddRegionSystem(IEcsProvider ecsProvider, CountryFactory factory)
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
                Add(_requestPool.Get(entity));
        }

        private void Add(CountryAddRegionRequest request)
        {
            if (!_linkPool.Has(request.RegionEntity))
                _factory.Create(request.RegionEntity);

            ref var link = ref _linkPool.Get(request.RegionEntity);
            ref var country = ref _pool.Get(link.CountryEntity);
            country.RegionsEntities.Add(request.RegionEntity);
        }
    }
}