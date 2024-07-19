using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.UI.Controllers;
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

        public RegionFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            var world = _ecsProvider.GetWorld();
            _eventsBus = _ecsProvider.GetEventsBus();
            _addRequestFilter = _eventsBus.GetEventBodies(out _addRequestPool);
            _removeRequestFilter = _eventsBus.GetEventBodies(out _removeRequestPool);
            _linkPool = world.GetPool<RegionLink>();
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