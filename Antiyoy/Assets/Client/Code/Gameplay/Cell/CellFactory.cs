using System.Collections.Generic;
using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Hex;
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
        private Transform _cellsRoot;
        private EcsPool<CellComponent> _pool;
        private EcsFilter _filter;
        private EcsWorld _world;
        private CellObject _cellPrefab;
        private MapProgressData _progress;

        public CellFactory(IEcsProvider ecsProvider, IStaticDataProvider staticData)
        {
            _ecsProvider = ecsProvider;
            _staticData = staticData;
        }

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _pool = _world.GetPool<CellComponent>();
            _filter = _world.Filter<CellComponent>().End();
            _cellsRoot = new GameObject("CellsRoot").transform;
            _cellPrefab = _staticData.Prefabs.CellObject;
        }

        public CellObject[] Create()
        {
            var width = _progress.Width;
            var height = _progress.Height;
            var cells = new CellObject[width * height];

            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                var index = i * width + j;
                cells[index] = CreateCell(new Vector2Int(i, j), index);
            }

            ConnectCells(cells, width, height);
            return cells;
        }

        private void ConnectCells(CellObject[] cells, int width, int height)
        {
            //TODO: попытаться оптимизировать используя hex.ArrayIndex и cells[]
            foreach (var cellObject in cells)
            foreach (var direction in HexUtilities.Directions)
            {
                ref var cell = ref _pool.Get(cellObject.Entity);
                var neighbourHex = cell.Hex + direction;
                var arrayIndex = neighbourHex.ToArrayIndex();

                if (arrayIndex.x < 0 || arrayIndex.x >= width ||
                    arrayIndex.y < 0 || arrayIndex.y >= height)
                    continue;

                cell.NeighbourCellEntities.Add(_pool.Find(_filter, c => c.Hex == neighbourHex));
            }
        }

        private CellObject CreateCell(Vector2Int arrayIndex, int index)
        {
            var hex = HexUtilities.FromArrayIndex(arrayIndex);

            var entity = _world.NewEntity();
            ref var cell = ref CreateCellComponent(index, entity, hex);
            var cellObject = CreateObject(hex);

            cellObject.Entity = entity;
            cell.Object = cellObject;

            return cellObject;
        }

        private ref CellComponent CreateCellComponent(int index, int entity, HexCoordinates hex)
        {
            ref var cell = ref _pool.Add(entity);
            cell.Hex = hex;
            cell.NeighbourCellEntities = new List<int>();
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

        public void OnLoad(MapProgressData progress) => _progress = progress;

        public UniTask OnSave(MapProgressData progress)
        {
            progress.Width = _progress.Width;
            progress.Height = _progress.Height;
            return UniTask.CompletedTask;
        }
    }
}