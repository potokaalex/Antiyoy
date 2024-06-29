using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Code.Cell
{
    //система отвечающая за создание клеток. Работает на запросах(событиях)

    public class CellCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private readonly ConfigProvider _configProvider;
        
        private EventsBus _eventsBus;
        private EcsFilter _requestFilter;
        private EcsPool<CellCreateRequest> _requestPool;
        private Transform _cellsRoot;

        public CellCreateSystem(EcsProvider ecsProvider, ConfigProvider configProvider)
        {
            _ecsProvider = ecsProvider;
            _configProvider = configProvider;
        }

        public void Init(IEcsSystems systems)
        {
            _eventsBus = _ecsProvider.GetEventsBus();
            _requestFilter = _eventsBus.GetEventBodies(out _requestPool);
            _cellsRoot = new GameObject("CellsRoot").transform;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requestFilter) 
                CreateCell(systems.GetWorld(), _requestPool.Get(requestEntity));
        }
        
        private void CreateCell(EcsWorld world, CellCreateRequest request)
        {
            var cellConfig = _configProvider.GetCell();

            var entity = world.NewEntity();
            var pool = world.GetPool<CellComponent>();
            pool.Add(entity);

            var position = GetPositionFromIndexes(request.Index);
            var cellObject = Object.Instantiate(cellConfig.Prefab, position, Quaternion.identity);
            cellObject.transform.SetParent(_cellsRoot);
            cellObject.name = $"Cell({request.Index})";
        }

        private Vector2 GetPositionFromIndexes(int index)
        {
            const float sideLength = 1f / 2;
            const float bigRadius = sideLength;
            const float smallRadius = sideLength * 0.8660254038f; //0.8660254038 = sqrt(3) / 2

            var mapWidth = _configProvider.GetMap().Width;
            var i = index % mapWidth;
            var j = index / mapWidth;
            var x = i * bigRadius * 1.5f;
            var y = j * smallRadius * 2;

            if (i % 2 != 0)
                y += smallRadius;

            return new Vector2(x, y);
        }
    }
}