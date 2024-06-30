using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Code.Tile
{
    public class TileCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileCreateRequest> _requestPool;
        private EcsFilter _requestFilter;
        private EcsPool<TileComponent> _pool;

        public TileCreateSystem(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _eventsBus = _ecsProvider.GetEventsBus();
            _requestFilter = _eventsBus.GetEventBodies(out _requestPool);
            _pool = world.GetPool<TileComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requestFilter)
            {
                var request = _requestPool.Get(requestEntity);
                request.Cell.SpriteRenderer.color = Color.white;
                _pool.Add(request.Cell.Entity);
            }
        }
    }
}