using System.Collections.Generic;
using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Ecs;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Utilities;
using ClientCode.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace ClientCode.Gameplay.Cell
{
    public class CellFactory : IInitializable, IProgressReader<MapProgressData>, IProgressWriter<MapProgressData>
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly IStaticDataProvider _staticData;
        private readonly GridManager _gridManager;
        private EcsPool<CellComponent> _pool;
        private EcsWorld _world;
        private MapProgressData _progress;

        public CellFactory(IEcsProvider ecsProvider, IStaticDataProvider staticData, GridManager gridManager)
        {
            _ecsProvider = ecsProvider;
            _staticData = staticData;
            _gridManager = gridManager;
        }

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _pool = _world.GetPool<CellComponent>();
            _world.Filter<CellComponent>().End();
        }

        public int[] Create()
        {
            var cells = CreateCells();
            CreateGrid(cells);
            ConnectCells(cells);
            CreateDebug(cells);

            return cells;
        }

        private void CreateGrid(int[] cells)
        {
            var gridPrefab = _staticData.Prefabs.GridObject;
            var emptyTile = _staticData.Prefabs.EmptyTile;
            var gridObject = Object.Instantiate(gridPrefab);

            _gridManager.Initialize(gridObject, cells, _progress.Size);
            _gridManager.FillByTile(_progress.Size, emptyTile);
        }

        private int[] CreateCells()
        {
            var height = _progress.Size.y;
            var width = _progress.Size.x;
            var cells = new int[width * height];

            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
            {
                var position = new Vector2Int(i, j);
                var arrayIndex = position.ToArrayIndex(width);
                cells[arrayIndex] = CreateCell(arrayIndex, position);
            }

            return cells;
        }

        private int CreateCell(int index, Vector2Int position)
        {
            var entity = _world.NewEntity();
            ref var cell = ref _pool.Add(entity);

            cell.NeighbourCellEntities = new List<int>(6);
            cell.Id = index;
            cell.GridPosition = position;

            return entity;
        }

        private void CreateDebug(int[] cells)
        {
#if DEBUG_PROJECT
            var root = new GameObject("CellDebugRoot").transform;
            var prefab = _staticData.Prefabs.CellDebug;

            var height = _progress.Size.y;
            var width = _progress.Size.x;

            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
            {
                var arrayIndex = new Vector2Int(i, j).ToArrayIndex(width);
                var entity = cells[arrayIndex];
                ref var cell = ref _pool.Get(entity);
                var position = _gridManager.GetCellWorldPosition(cell.GridPosition);
                var instance = Object.Instantiate(prefab, position, Quaternion.identity);
                instance.name = entity.ToString();
                instance.transform.SetParent(root);
                instance.Neighbours = cell.NeighbourCellEntities;
                cell.Debug = instance;
            }
#endif
        }

        private void ConnectCells(int[] cells)
        {
            var height = _progress.Size.y;
            var width = _progress.Size.x;

            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
            {
                var position = new Vector2Int(i, j);
                var arrayIndex = position.ToArrayIndex(width);
                ref var cell = ref _pool.Get(cells[arrayIndex]);

                foreach (var direction in HexDirectionsUtilities.GetNeighbors(position))
                {
                    var neighborPosition = position + direction;

                    if (neighborPosition.x < 0 || neighborPosition.y < 0 || neighborPosition.x >= width || neighborPosition.y >= height)
                        continue;

                    var neighborArrayIndex = neighborPosition.ToArrayIndex(width);

                    cell.NeighbourCellEntities.Add(cells[neighborArrayIndex]);
                }
            }
        }

        public void OnLoad(MapProgressData progress) => _progress = progress;

        public UniTask OnSave(MapProgressData progress)
        {
            progress.Size = _progress.Size;
            return UniTask.CompletedTask;
        }
    }
}