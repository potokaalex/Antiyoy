using System.Collections.Generic;
using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Ecs;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.StaticDataProvider;
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
            var gridPrefab = _staticData.Prefabs.GridObject;
            var emptyTile = _staticData.Prefabs.EmptyTile;
            var gridObject = Object.Instantiate(gridPrefab);
            var cells = CreateCells();

            _gridManager.Initialize(gridObject, cells, _progress.Size);
            _gridManager.FillByTile(_progress.Size, emptyTile);

            return cells;
        }

        private int[] CreateCells()
        {
            var width = _progress.Size.x;
            var height = _progress.Size.y;
            var cells = new int[width * height];

            for (var i = 0; i < _progress.Size.y; i++)
            for (var j = 0; j < _progress.Size.x; j++)
            {
                var gridPosition = new Vector2Int(i, j);
                var arrayIndex = gridPosition.ToArrayIndex(width);
                cells[arrayIndex] = CreateCell(arrayIndex, gridPosition);
            }

            return cells;
        }

        private int CreateCell(int index, Vector2Int gridPosition)
        {
            var entity = _world.NewEntity();
            ref var cell = ref _pool.Add(entity);

            cell.NeighbourCellEntities = new List<int>(6);
            cell.Id = index;
            cell.GridPosition = gridPosition;

            return entity;
        }

        /*
        private void ConnectCells(CellObject[] cells, int width, int height)
        {
            //BUG!
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                var arrayIndex = new Vector2Int(i, j);
                var index = GetIndex(i, j, width);

                foreach (var direction in HexUtilities.Directions)
                {
                    var neighbourArrayIndex = (HexUtilities.FromArrayIndex(arrayIndex) + direction).ToArrayIndex();
                    var neighbourIndex = GetIndex(neighbourArrayIndex.x, neighbourArrayIndex.y, width);

                    if (neighbourIndex >= 0 && neighbourIndex < width * height)
                        cells[index].NeighbourCellEntities.Add(cells[neighbourIndex].Entity);
                }
            }
        }

        private ref CellComponent CreateCellComponent(int index, int entity)
        {
            ref var cell = ref _pool.Add(entity);
            cell.NeighbourCellEntities = new List<int>(6);
            cell.Id = index;
            return ref cell;
        }

        private CellObject CreateObject(HexCoordinates hex)
        {
            var position = hex.ToWorldPosition();
            var cellObject = Object.Instantiate(_cellPrefab, position, Quaternion.identity);
            cellObject.transform.SetParent(_cellsRoot);
            cellObject.name = $"Cell({hex})";
            return cellObject;
        }
*/

        //private int GetIndex(int i, int j, int width) => i * width + j;

        public void OnLoad(MapProgressData progress) => _progress = progress;

        public UniTask OnSave(MapProgressData progress)
        {
            progress.Size = _progress.Size;
            return UniTask.CompletedTask;
        }
    }
}