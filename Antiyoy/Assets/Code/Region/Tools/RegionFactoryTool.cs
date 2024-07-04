using Code.Region.Components;
using Leopotam.EcsLite;

namespace Code.Region.Tools
{
    public static class RegionFactoryTool
    {
        public static int Create(EcsWorld world, EcsPool<RegionComponent> pool, int cellEntitiesCount = 0)
        {
            var entity = world.NewEntity();
            ref var region = ref pool.Add(entity);
            region.CellEntities = ListPool<int>.Get(cellEntitiesCount);
            return entity;
        }

        public static void Destroy(int regionEntity, EcsPool<RegionComponent> pool)
        {
            ListPool<int>.Release(pool.Get(regionEntity).CellEntities);
            pool.Del(regionEntity);
        }
    }
}