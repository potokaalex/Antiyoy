using ClientCode.Gameplay.Region.Components;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;

namespace ClientCode.Gameplay.Region.Tools
{
    public static class RegionAddCellTool
    {
        public static void AddCell(int cellEntity, int regionEntity, EcsPool<RegionLink> linkPool,
            EcsPool<RegionComponent> pool)
        {
            ref var link = ref linkPool.GetOrAdd(cellEntity);
            link.RegionEntity = regionEntity;

            ref var region = ref pool.Get(regionEntity);
            region.CellEntities.Add(cellEntity);
        }
    }
}