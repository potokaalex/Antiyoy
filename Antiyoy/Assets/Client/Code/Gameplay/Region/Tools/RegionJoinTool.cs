using System.Collections.Generic;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Tools
{
    public static class RegionJoinTool
    {
        //moves data to a major region from non-major regions. Deletes non-major regions.
        public static int Join(List<int> regions, EcsPool<RegionComponent> pool, EcsPool<RegionLink> linkPool, EventsBus events)
        {
            var majorRegion = GetMajorRegion(regions, pool);

            foreach (var regionEntity in regions)
            {
                if (regionEntity == majorRegion)
                    continue;

                var region = pool.Get(regionEntity);

                foreach (var cell in region.CellEntities)
                    RegionAddCellTool.AddCell(cell, majorRegion, linkPool, pool);

                RegionFactoryTool.Destroy(regionEntity, pool, events);
            }

            return majorRegion;
        }

        private static int GetMajorRegion(List<int> regions, EcsPool<RegionComponent> pool)
        {
            var majorRegionEntity = regions[0];

            for (var i = 1; i < regions.Count; i++)
            {
                var mainRegion = pool.Get(majorRegionEntity);
                var region = pool.Get(regions[i]);

                if (region.CellEntities.Count > mainRegion.CellEntities.Count)
                    majorRegionEntity = regions[i];
            }

            return majorRegionEntity;
        }
    }
}