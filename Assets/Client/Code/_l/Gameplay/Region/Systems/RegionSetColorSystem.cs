using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClientCode.Gameplay.Region.Systems
{
    public class RegionSetColorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly GridManager _gridManager;
        private EcsPool<RegionAddCellRequest> _regionAddCellRequestPool;
        private EcsFilter _regionAddCellRequestFilter;
        private EcsPool<CellComponent> _cellPool;

        public RegionSetColorSystem(IEcsProvider ecsProvider, GridManager gridManager)
        {
            _ecsProvider = ecsProvider;
            _gridManager = gridManager;
        }

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();
            var eventsBus = _ecsProvider.GetEventsBus();
            _regionAddCellRequestFilter = eventsBus.GetEventBodies(out _regionAddCellRequestPool);
            _cellPool = world.GetPool<CellComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _regionAddCellRequestFilter)
            {
                var request = _regionAddCellRequestPool.Get(entity);
                var cell = _cellPool.Get(request.CellEntity);
                var colors = new Dictionary<RegionType, Color>(); // _staticData.Configs.Gameplay.RegionColors;
                _gridManager.SetColor(cell.GridPosition, colors[request.Type]);
            }
        }
    }
}