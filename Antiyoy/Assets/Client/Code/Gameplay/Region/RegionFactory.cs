using System.Collections.Generic;
using ClientCode.Data.Saved;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region
{
    public class RegionFactory
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<RegionAddCellRequest> _addRequestPool;
        private EcsFilter _addRequestFilter;
        private EcsPool<RegionRemoveCellRequest> _removeRequestPool;
        private EcsFilter _removeRequestFilter;
        private EcsPool<RegionLink> _linkPool;
        private EcsWorld _world;

        public RegionFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _eventsBus = _ecsProvider.GetEventsBus();
            _addRequestFilter = _eventsBus.GetEventBodies(out _addRequestPool);
            _removeRequestFilter = _eventsBus.GetEventBodies(out _removeRequestPool);
            _linkPool = _world.GetPool<RegionLink>();
            _world.GetPool<RegionComponent>();
        }

        public void Create(List<RegionSavedData> regions, CellObject[] cells)
        {
            foreach (var region in regions)
            foreach (var cellId in region.CellsId)
                Create(cells[cellId].Entity, region.Type);
        }

        public void Create(int cell, RegionType type)
        {
            if (_addRequestPool.Has(_addRequestFilter, r => r.CellEntity == cell && r.Type == type))
                return;

            Destroy(cell);

            ref var regionRequest = ref _eventsBus.NewEvent<RegionAddCellRequest>();
            regionRequest.CellEntity = cell;
            regionRequest.Type = type;
        }

        public void Destroy(int cell)
        {
            if (_removeRequestPool.Has(_removeRequestFilter, r => r.CellEntity == cell))
                return;

            if (!_linkPool.Has(cell))
                return;

            ref var regionRequest = ref _eventsBus.NewEvent<RegionRemoveCellRequest>();
            regionRequest.CellEntity = cell;
        }
    }
}