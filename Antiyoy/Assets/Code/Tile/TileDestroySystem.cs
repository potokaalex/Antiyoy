using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Code.Tile
{
    public class TileDestroySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileDestroyRequest> _requestPool;
        private EcsFilter _requestFilter;
        private EcsPool<TileComponent> _pool;

        public TileDestroySystem(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();
            
            _eventsBus = _ecsProvider.GetEventsBus();
            _requestFilter = _eventsBus.GetEventBodies(out _requestPool);
            _pool = world.GetPool<TileComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requestFilter)
            {
                var request = _requestPool.Get(requestEntity);
                request.Cell.SpriteRenderer.color = Color.black;
                _pool.Del(request.Cell.Entity);
            }
        }
    }
}