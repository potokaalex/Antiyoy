using System.Collections.Generic;
using Client.Code.Services;
using Client.Code.Services.Config;
using ClientCode.Gameplay.Cell;
using ClientCode.UI.Windows.Writing;
using ClientCode.Utilities;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Client.Code.Gameplay
{
    public class CellsFactory : IInitializable
    {
        private readonly MapsContainer _mapsContainer;
        private readonly EcsController _ecsController;
        private readonly IConfigsProvider _configsProvider;
        private readonly Instantiator _instantiator;
        private EcsPool<CellComponent> _pool;

        public CellsFactory(MapsContainer mapsContainer, EcsController ecsController, IConfigsProvider configsProvider, Instantiator instantiator)
        {
            _instantiator = instantiator;
            _configsProvider = configsProvider;
            _ecsController = ecsController;
            _mapsContainer = mapsContainer;
        }

        public void Initialize() => _pool = _ecsController.World.GetPool<CellComponent>();

        public int[] CreateEntitiesWithCells(Grid grid)
        {
            var size = _mapsContainer.CurrentMap.Size;
            var cells = CreateCells(size);
            ConnectCells(cells, size);
#if UNITY_EDITOR
            CreateDebug(cells, size, grid);
#endif
            return cells;
        }

        private int[] CreateCells(Vector2Int size)
        {
            var cells = new int[size.x * size.y];

            for (var i = 0; i < size.y; i++)
            for (var j = 0; j < size.x; j++)
            {
                var position = new Vector2Int(i, j);
                var arrayIndex = position.ToArrayIndex(size.x);
                cells[arrayIndex] = CreateCell(arrayIndex, position);
            }

            return cells;
        }

        private int CreateCell(int index, Vector2Int position)
        {
            var entity = _ecsController.World.NewEntity();
            ref var cell = ref _pool.Add(entity);

            cell.NeighbourCellEntities = new List<int>(6);
            cell.Id = index;
            cell.GridPosition = position;

            return entity;
        }

        private void ConnectCells(int[] cells, Vector2Int size)
        {
            for (var i = 0; i < size.y; i++)
            for (var j = 0; j < size.x; j++)
            {
                var position = new Vector2Int(i, j);
                var arrayIndex = position.ToArrayIndex(size.x);
                ref var cell = ref _pool.Get(cells[arrayIndex]);

                foreach (var direction in HexDirectionsUtilities.GetNeighborsDirections(position))
                {
                    var neighborPosition = position + direction;

                    if (neighborPosition.x < 0 || neighborPosition.y < 0 || neighborPosition.x >= size.x || neighborPosition.y >= size.y)
                        continue;

                    var neighborArrayIndex = neighborPosition.ToArrayIndex(size.x);

                    cell.NeighbourCellEntities.Add(cells[neighborArrayIndex]);
                }
            }
        }

        private void CreateDebug(int[] cells, Vector2Int size, Grid grid)
        {
            var root = new GameObject("CellsDebugRoot").transform;
            var prefab = _configsProvider.Data.CellDebugPrefab;

            for (var i = 0; i < size.y; i++)
            for (var j = 0; j < size.x; j++)
            {
                var arrayIndex = new Vector2Int(i, j).ToArrayIndex(size.x);
                var entity = cells[arrayIndex];
                ref var cell = ref _pool.Get(entity);
                var position = grid.GetCellCenterWorld((Vector3Int)cell.GridPosition);
                var instance = _instantiator.InstantiateForComponent<CellDebugBehaviour>(prefab.gameObject, position, Quaternion.identity);
                instance.name = entity.ToString();
                instance.transform.SetParent(root);
                instance.Neighbours = cell.NeighbourCellEntities;
                cell.Debug = instance;
            }
        }
    }
}