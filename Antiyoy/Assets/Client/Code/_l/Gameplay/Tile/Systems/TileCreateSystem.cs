using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.StaticDataProvider;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace ClientCode.Gameplay.Tile.Systems
{
    public class TileCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly GridManager _gridManager;
        private readonly IStaticDataProvider _staticData;
        private EventsBus _eventsBus;
        private EcsPool<TileCreateRequest> _requestPool;
        private EcsFilter _requestFilter;
        private EcsPool<TileComponent> _pool;
        private EcsPool<CellComponent> _cellPool;

        public TileCreateSystem(IEcsProvider ecsProvider, GridManager gridManager, IStaticDataProvider staticData)
        {
            _ecsProvider = ecsProvider;
            _gridManager = gridManager;
            _staticData = staticData;
        }

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();

            _eventsBus = _ecsProvider.GetEventsBus();
            _requestFilter = _eventsBus.GetEventBodies(out _requestPool);
            _pool = world.GetPool<TileComponent>();
            _cellPool = world.GetPool<CellComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requestFilter)
                CreateTile(_requestPool.Get(requestEntity));
        }

        private void CreateTile(TileCreateRequest request)
        {
            _pool.Add(request.CellEntity);

            var tile = _staticData.Prefabs.Tile;
            var cell = _cellPool.Get(request.CellEntity);
            _gridManager.SetTile(cell.GridPosition, tile);
        }
    }
}