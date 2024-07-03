using System.Collections.Generic;
using Code.Region.Components;
using Leopotam.EcsLite;

namespace Code.Region.Tools
{
    public static class RegionDivideTool
    {
        //отделяем от major региона тайлы и создаёт из них новые регионы
        public static void Divide(List<RegionPart> regionParts, List<int> baseRegionCellEntities, EcsWorld world,
            EcsPool<RegionComponent> pool, EcsPool<RegionLink> linkPool)
        {
            var majorPart = GetMajorPart(regionParts);

            foreach (var part in regionParts)
            {
                if (part.Cells == majorPart.Cells)
                    continue;

                var newRegionEntity = RegionFactoryTool.Create(world, pool, part.Cells.Count);
                ref var newRegion = ref pool.Get(newRegionEntity);

                MoveCells(part, newRegion, baseRegionCellEntities, newRegionEntity, linkPool);
            }
        }

        private static void MoveCells(RegionPart part, RegionComponent newRegion, List<int> baseRegionCellEntities,
            int newRegionEntity, EcsPool<RegionLink> linkPool)
        {
            foreach (var cell in part.Cells)
            {
                newRegion.CellEntities.Add(cell);
                baseRegionCellEntities.Remove(cell);

                ref var link = ref linkPool.Get(cell);
                link.RegionEntity = newRegionEntity;
            }
        }

        private static RegionPart GetMajorPart(List<RegionPart> parts)
        {
            var majorPart = parts[0];

            for (var i = 1; i < parts.Count; i++)
            {
                var part = parts[i];

                if (part.Cells.Count > majorPart.Cells.Count)
                    majorPart = part;
            }

            return majorPart;
        }
    }
}