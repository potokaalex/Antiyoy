using System.Collections.Generic;
using ClientCode.Data.Saved;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Hex;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;

namespace ClientCode.Services.Progress.Map
{
    public class MapDataFactory
    {
        private readonly IEcsProvider _ecsProvider;
        private EcsPool<RegionComponent> _regionPool;
        private EcsPool<CellComponent> _cellPool;

        public MapDataFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            var world = _ecsProvider.GetWorld();
            _cellPool = world.GetPool<CellComponent>();
            _regionPool = world.GetPool<RegionComponent>();
        }
        
        public TileSavedData CreateTileData(int entity, int mapHeight)
        {
            var id = GetCellId(entity, mapHeight);
            var savedData = new TileSavedData
            {
                Id = id
            };
            return savedData;
        }
        
        public RegionSavedData CreateRegionData(int entity, int mapHeight)
        {
            var region = _regionPool.Get(entity);

            var regionSavedData = new RegionSavedData
            {
                CellsId = new List<int>()
            };

            foreach (var cellEntity in region.CellEntities)
                regionSavedData.CellsId.Add(GetCellId(cellEntity, mapHeight));

            return regionSavedData;
        }
        
        private int GetCellId(int entity, int mapHeight)
        {
            var cell = _cellPool.Get(entity);
            var arrayIndex = cell.Hex.ToArrayIndex();
            var id = arrayIndex.x * mapHeight + arrayIndex.y;
            return id;
        }
    }
}