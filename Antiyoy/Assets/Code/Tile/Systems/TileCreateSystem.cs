using Code.Ecs;
using Code.Tile.Components;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Code.Tile.Systems
{
    public class TileCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileCreateRequest> _requestPool;
        private EcsFilter _requestFilter;
        private EcsPool<TileComponent> _pool;

        public TileCreateSystem(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

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
                CreateTile(_requestPool.Get(requestEntity));
        }

        private void CreateTile(TileCreateRequest request)
        {
            var thisEntity = request.Cell.Entity;
            _pool.Add(thisEntity);
            request.Cell.SpriteRenderer.color = Color.white;
        }
    }
}