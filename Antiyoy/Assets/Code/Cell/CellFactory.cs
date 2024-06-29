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

            CreateCells();
        }

        private void CreateCells()
        {
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < 10; j++)
            {
                CreateCell(i, j);
            }
        }

        private void CreateCell(int i, int j)
        {
            var world = _ecsProvider.GetWorld();
            var cellConfig = _configProvider.GetCell();

            var entity = world.NewEntity();
            var pool = world.GetPool<CellComponent>();
            pool.Add(entity); //ref var cellComponent = ref pool.Add(entity);

            var position = GetPositionFromIndexes(i, j);
            var cellObject = Object.Instantiate(cellConfig.Prefab, position, Quaternion.identity);
            cellObject.transform.SetParent(_cellsRoot);
            cellObject.name = $"Cell({i},{j})";
        }

        private Vector2 GetPositionFromIndexes(int i, int j)
        {
            const float sideLength = 1f / 2;
            const float bigRadius = sideLength;
            const float smallRadius = sideLength * 0.8660254038f; //0.8660254038 = sqrt(3) / 2

            var x = i * bigRadius * 1.5f;
            var y = j * smallRadius * 2;

            if (i % 2 != 0)
                y += smallRadius;

            return new Vector2(x, y);
        }
    }
}