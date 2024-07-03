using System.Collections.Generic;
using Code.Region.Components;
using Leopotam.EcsLite;

namespace Code.Region.Tools
{
    public static class RegionJoinTool
    {
        //перемещает данные из других регионов в major и удаляет другие регионы.
        public static int Join(List<int> regions, EcsPool<RegionComponent> pool,
            EcsPool<RegionLink> linkPool)
        {
            var majorRegion = GetMajorRegion(regions, pool);

            foreach (var regionEntity in regions)
            {
                if (regionEntity == majorRegion)
                    continue;

                var region = pool.Get(regionEntity);

                foreach (var cell in region.CellEntities)
                    RegionAddCellTool.AddCell(cell, majorRegion, linkPool, pool);

                RegionFactoryTool.Destroy(regionEntity, pool);
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