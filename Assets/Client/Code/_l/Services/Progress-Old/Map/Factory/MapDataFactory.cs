using System.Collections.Generic;
using ClientCode.Data.Saved;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;

namespace ClientCode.Services.Progress.Map.Factory
{
    public static class ProgressDataFactory
    {
        public static TileSavedData CreateTileData(int entity, EcsPool<CellComponent> cellPool)
        {
            var id = cellPool.Get(entity).Id;
            var savedData = new TileSavedData
            {
                Id = id
            };
            return savedData;
        }

        public static RegionSavedData CreateRegionData(int entity, EcsPool<RegionComponent> pool, EcsPool<CellComponent> cellPool)
        {
            var region = pool.Get(entity);

            var regionSavedData = new RegionSavedData
            {
                CellsId = new List<int>(),
                Type = region.Type
            };

            for (var i = 0; i < region.CellEntities.Count; i++)
            {
                var id = cellPool.Get(region.CellEntities[i]).Id;
                regionSavedData.CellsId.Add(id);
            }

            return regionSavedData;
        }
    }
}