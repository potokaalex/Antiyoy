using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Code.Tile
{
    public class TileCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private readonly ConfigProvider _configProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileCreateRequest> _requestPool;
        private EcsFilter _requestFilter;
        private EcsPool<TileComponent> _pool;
        private EcsFilter _filer;

        public TileCreateSystem(EcsProvider ecsProvider, ConfigProvider configProvider)
        {
            _ecsProvider = ecsProvider;
            _configProvider = configProvider;
        }

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();
            
            _eventsBus = _ecsProvider.GetEventsBus();
            _requestFilter = _eventsBus.GetEventBodies(out _requestPool);
            _pool = world.GetPool<TileComponent>();
            _filer = world.Filter<TileComponent>().End();
            //_mapWidth
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requestFilter) 
                CreateTile(_requestPool.Get(requestEntity));
        }

        private void CreateTile(TileCreateRequest request)
        {
            var thisEntity = request.Cell.Entity;
            ref var tile = ref _pool.Add(thisEntity);
            //из индекса получить индекс другой клетки и только теперь по индексу другой клетки отыскать нужную нам.

           // _pool.Has()
            
            request.Cell.SpriteRenderer.color = Color.white;
            //request.Cell.DebugText.text = thisEntity.ToString();
        }
    }
}