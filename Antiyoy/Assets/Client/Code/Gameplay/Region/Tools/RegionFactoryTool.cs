using ClientCode.Gameplay.Countries.Components;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Utilities;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Tools
{
    public static class RegionFactoryTool
    {
        public static int Create(EcsWorld world, EcsPool<RegionComponent> pool, RegionType type, EventsBus events, int cellEntitiesCount = 0)
        {
            var entity = world.NewEntity();
            ref var region = ref pool.Add(entity);
            region.CellEntities = ListPool<int>.Get(cellEntitiesCount);
            region.Type = type;
            events.NewEvent<CountryAddRegionRequest>().RegionEntity = entity;
            return entity;
        }

        public static void Destroy(int regionEntity, EcsPool<RegionComponent> pool, EventsBus events)
        {
            ListPool<int>.Release(pool.Get(regionEntity).CellEntities);
            pool.Del(regionEntity);
            events.NewEvent<CountryRemoveRegionRequest>().RegionEntity = regionEntity;
        }
    }
}