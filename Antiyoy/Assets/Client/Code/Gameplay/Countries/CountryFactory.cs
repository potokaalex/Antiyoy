using System.Collections.Generic;
using ClientCode.Gameplay.Countries.Components;
using ClientCode.Gameplay.Countries.Systems;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;
using Zenject;

namespace ClientCode.Gameplay.Countries
{
    public class CountryFactory : IInitializable
    {
        private readonly IEcsProvider _ecsProvider;
        private EcsWorld _world;
        private EcsPool<CountryComponent> _pool;
        private EcsPool<RegionComponent> _regionPool;
        private EcsPool<CountryLink> _linkPool;

        public CountryFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _pool = _world.GetPool<CountryComponent>();
            _regionPool = _world.GetPool<RegionComponent>();
            _linkPool = _world.GetPool<CountryLink>();
        }
        
        public void Create(int regionEntity)
        {
            var region = _regionPool.Get(regionEntity);
            var countryEntity = CreateComponent(region);
            _linkPool.Add(regionEntity).CountryEntity = countryEntity;
        }

        public void Destroy(int regionEntity)
        {
            ref var link = ref _linkPool.Get(regionEntity);
            _pool.Del(link.CountryEntity);
            _linkPool.Del(regionEntity); 
        }

        private int CreateComponent(RegionComponent region)
        {
            var entity = _world.NewEntity();
            ref var country = ref _pool.Add(entity);
            country.RegionsEntities = new List<int>();
            country.Type = region.Type;
            return entity;
        }
    }
}