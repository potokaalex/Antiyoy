using System.Collections.Generic;
using ClientCode.Data.Progress.Player;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Hex;
using ClientCode.Services.StaticDataProvider;
using Leopotam.EcsLite;
using Plugins.EcsLite;
using UnityEngine;

namespace ClientCode.Gameplay.Cell
{
    public class CellFactory
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly IStaticDataProvider _staticDataProvider;
        private Transform _cellsRoot;
        private EcsPool<CellComponent> _pool;
        private EcsFilter _filter;
        private EcsWorld _world;
        private CellObject _cellPrefab;

        public CellFactory(IEcsProvider ecsProvider, IStaticDataProvider staticDataProvider)
        {
            _ecsProvider = ecsProvider;
            _staticDataProvider = staticDataProvider;
        }

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _pool = _world.GetPool<CellComponent>();
            _filter = _world.Filter<CellComponent>().End();
            _cellsRoot = new GameObject("CellsRoot").transform;
            _cellPrefab = _staticDataProvider.Prefabs.CellObject;
        }

        public void Create(MapProgressData mapData)
        {
            var cells = new CellObject[mapData.Width * mapData.Height];

            for (var i = 0; i < mapData.Width; i++)
            for (var j = 0; j < mapData.Height; j++)
            {
                var index = i * mapData.Width + j;
                var hex = HexUtilities.FromArrayIndex(new Vector2Int(i, j));
                cells[index] = CreateCell(hex);
            }

            ConnectCells(cells, mapData);
        }

        private void ConnectCells(CellObject[] cells, MapProgressData mapData)
        {
            foreach (var cellObject in cells)
            {
                foreach (var direction in HexUtilities.Directions)
                {
                    ref var cell = ref _pool.Get(cellObject.Entity);
                    var neighbourHex = cell.Hex + direction;
                    var arrayIndex = neighbourHex.ToArrayIndex();

                    if (arrayIndex.x < 0 || arrayIndex.x >= mapData.Width ||
                        arrayIndex.y < 0 || arrayIndex.y >= mapData.Height)
                        continue;

                    cell.NeighbourCellEntities.Add(_pool.Find(_filter, c => c.Hex == neighbourHex));
                }
            }
        }

        private CellObject CreateCell(HexCoordinates hex)
        {
            var entity = _world.NewEntity();
            ref var cell = ref _pool.Add(entity);
            cell.Hex = hex;
            cell.NeighbourCellEntities = new List<int>();

            var position = hex.ToWorldPosition();
            var cellObject = Object.Instantiate(_cellPrefab, position, Quaternion.identity);
            cellObject.transform.SetParent(_cellsRoot);
            cellObject.name = $"Cell({hex})";
            cellObject.Entity = entity;

            cell.Object = cellObject;

            return cellObject;
        }
    }
}