using UnityEngine;

namespace Code
{
    public class CellFactory
    {
        private readonly EcsProvider _ecsProvider;
        private readonly ConfigProvider _configProvider;

        public CellFactory(EcsProvider ecsProvider, ConfigProvider configProvider)
        {
            _ecsProvider = ecsProvider;
            _configProvider = configProvider;
        }

        public void Create()
        {
            CreateCells();
        }

        private void CreateCells()
        {
            var sideLength = 1f / 2;
            var bigRadius = sideLength;
            var smallRadius = sideLength * Mathf.Sqrt(3f) / 2f;

            for (var i = 0; i < 10; i++)
            {
                var x = i * bigRadius * 1.5f;
                var y = 0f;

                if (i % 2 != 0)
                    y = smallRadius;
                
                CreateCell(new Vector2(x, y));
            }
        }

        private void CreateCell(Vector2 position)
        {
            var world = _ecsProvider.GetWorld();
            var cellConfig = _configProvider.GetCell();
            
            var entity = world.NewEntity();
            var pool = world.GetPool<CellComponent>();
            pool.Add(entity); //ref var cellComponent = ref pool.Add(entity);

            Object.Instantiate(cellConfig.Prefab, position, Quaternion.identity);
        }
    }
}