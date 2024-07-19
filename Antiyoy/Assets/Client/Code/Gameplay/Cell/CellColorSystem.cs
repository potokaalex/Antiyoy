using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.StaticDataProvider;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClientCode.Gameplay.Cell
{
    public class CellColorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly IStaticDataProvider _staticData;
        private EcsPool<TileCreateRequest> _createRequestPool;
        private EcsFilter _createRequestFilter;
        private EcsPool<TileDestroyRequest> _destroyRequestPool;
        private EcsFilter _destroyRequestFilter;
        private EcsPool<RegionAddCellRequest> _regionAddCellRequestPool;
        private EcsFilter _regionAddCellRequestFilter;
        private EcsPool<CellComponent> _cellPool;

        public CellColorSystem(IEcsProvider ecsProvider, IStaticDataProvider staticData)
        {
            _ecsProvider = ecsProvider;
            _staticData = staticData;
        }

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();
            var eventsBus = _ecsProvider.GetEventsBus();
            _createRequestFilter = eventsBus.GetEventBodies(out _createRequestPool);
            _destroyRequestFilter = eventsBus.GetEventBodies(out _destroyRequestPool);
            _regionAddCellRequestFilter = eventsBus.GetEventBodies(out _regionAddCellRequestPool);
            _cellPool = world.GetPool<CellComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _createRequestFilter)
                _createRequestPool.Get(entity).Cell.SpriteRenderer.color = Color.white;

            foreach (var entity in _destroyRequestFilter)
                _destroyRequestPool.Get(entity).Cell.SpriteRenderer.color = Color.black;

            foreach (var entity in _regionAddCellRequestFilter)
            {
                var request = _regionAddCellRequestPool.Get(entity);
                var cell = _cellPool.Get(request.CellEntity);
                cell.Object.SpriteRenderer.color = _staticData.Configs.Region.Colors[request.Type];
            }
        }
    }
}