﻿using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Cell
{
    public class CellFactory
    {
        private readonly EcsProvider _ecsProvider;
        private readonly ConfigProvider _configProvider;
        private Transform _cellsRoot;

        public CellFactory(EcsProvider ecsProvider, ConfigProvider configProvider)
        {
            _ecsProvider = ecsProvider;
            _configProvider = configProvider;
        }

        public void Create()
        {
            _cellsRoot = new GameObject("CellsRoot").transform;
            
            var world = _ecsProvider.GetWorld();
            var mapConfig = _configProvider.GetMap();
            
            for (var i = 0; i < mapConfig.Height; i++)
            for (var j = 0; j < mapConfig.Width; j++)
            {
                var index = i * mapConfig.Width + j;
                CreateCell(world, index, GetPositionFromIndexes(i, j));
            }
        }

        private Vector2 GetPositionFromIndexes(int i, int j)
        {
            const float sideLength = 1f / 2;
            const float bigRadius = sideLength;
            const float smallRadius = sideLength * 0.8660254038f; //0.8660254038 = sqrt(3) / 2

            var x = j * bigRadius * 1.5f;
            var y = i * smallRadius * 2;

            if (j % 2 != 0)
                y += smallRadius;

            return new Vector2(x, y);
        }

        private void CreateCell(EcsWorld world, int index, Vector2 position)
        {
            var cellConfig = _configProvider.GetCell();

            var entity = world.NewEntity();
            var pool = world.GetPool<CellComponent>();
            pool.Add(entity);

            var cellObject = Object.Instantiate(cellConfig.Prefab, position, Quaternion.identity);
            cellObject.transform.SetParent(_cellsRoot);
            cellObject.name = $"Cell({index})";
            cellObject.Entity = entity;
        }
    }
}