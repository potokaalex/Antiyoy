using System.Collections.Generic;
using Code.Hex;
using Leopotam.EcsLite;
using Plugins.EcsLite;
using UnityEngine;

namespace Code.Cell
{
    public class CellFactory
    {
        private readonly EcsProvider _ecsProvider;
        private readonly ConfigProvider _configProvider;
        private Transform _cellsRoot;
        private EcsPool<CellComponent> _pool;
        private EcsFilter _filter;

        public CellFactory(EcsProvider ecsProvider, ConfigProvider configProvider)
        {
            _ecsProvider = ecsProvider;
            _configProvider = configProvider;
        }

        public void Create()
        {
            var world = _ecsProvider.GetWorld();
            var mapConfig = _configProvider.GetMap();
            var cells = new CellObject[mapConfig.Width * mapConfig.Height];

            _pool = world.GetPool<CellComponent>();
            _filter = world.Filter<CellComponent>().End();
            _cellsRoot = new GameObject("CellsRoot").transform;

            for (var i = 0; i < mapConfig.Width; i++)
            for (var j = 0; j < mapConfig.Height; j++)
            {
                var index = i * mapConfig.Width + j;
                var hex = HexUtilities.FromArrayIndex(new Vector2Int(i, j));
                cells[index] = CreateCell(world, hex);
            }

            ConnectCells(cells, mapConfig);
        }

        private void ConnectCells(CellObject[] cells, MapConfig mapConfig)
        {
            foreach (var cellObject in cells)
            {
                foreach (var direction in HexUtilities.Directions)
                {
                    ref var cell = ref _pool.Get(cellObject.Entity);
                    var neighbourHex = cell.Hex + direction;
                    var arrayIndex = neighbourHex.ToArrayIndex();
                    
                    if (arrayIndex.x < 0 || arrayIndex.x >= mapConfig.Width || 
                        arrayIndex.y < 0 || arrayIndex.y >= mapConfig.Height)
                        continue;

                    cell.NeighboursCellsEntities.Add(_pool.Find(_filter, c => c.Hex == neighbourHex));
                }
            }
        }

        private CellObject CreateCell(EcsWorld world, HexCoordinates hex)
        {
            var cellConfig = _configProvider.GetCell();

            var entity = world.NewEntity();
            ref var cell = ref _pool.Add(entity);
            cell.Hex = hex;
            cell.NeighboursCellsEntities = new List<int>();
            
            var position = hex.ToWorldPosition();
            var cellObject = Object.Instantiate(cellConfig.Prefab, position, Quaternion.identity);
            cellObject.transform.SetParent(_cellsRoot);
            cellObject.name = $"Cell({hex})";
            cellObject.Entity = entity;
            //cellObject.DebugText.text = $"{hex}\n{hex.ToArrayIndex()}";

            cell.Object = cellObject;
            
            return cellObject;
        }
    }
}