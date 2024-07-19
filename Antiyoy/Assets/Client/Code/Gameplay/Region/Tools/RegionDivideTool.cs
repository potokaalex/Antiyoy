using System.Collections.Generic;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Systems;
using Leopotam.EcsLite;

namespace ClientCode.Gameplay.Region.Tools
{
    public static class RegionDivideTool
    {
        //separates non-major regionParts from baseRegionCells. Creates new regions from non-major parts.
        public static void Divide(List<RegionPart> regionParts, List<int> baseRegionCells, EcsWorld world,
            EcsPool<RegionComponent> pool, EcsPool<RegionLink> linkPool, RegionType type)
        {
            var majorPart = GetMajorPart(regionParts);

            foreach (var part in regionParts)
            {
                if (part.Cells == majorPart.Cells)
                    continue;

                var newRegionEntity = RegionFactoryTool.Create(world, pool, type, part.Cells.Count);
                ref var newRegion = ref pool.Get(newRegionEntity);

                MoveCells(part, newRegion, baseRegionCells, newRegionEntity, linkPool);
            }
        }

        private static void MoveCells(RegionPart part, RegionComponent newRegion, List<int> baseRegionCells,
            int newRegionEntity, EcsPool<RegionLink> linkPool)
        {
            foreach (var cell in part.Cells)
            {
                newRegion.CellEntities.Add(cell);
                baseRegionCells.Remove(cell);

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