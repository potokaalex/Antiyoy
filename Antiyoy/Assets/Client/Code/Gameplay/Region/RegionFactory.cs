using System.Collections.Generic;
using ClientCode.Data.Saved;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Tools;
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
        private EcsPool<RegionComponent> _pool;

        public RegionFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _eventsBus = _ecsProvider.GetEventsBus();
            _addRequestFilter = _eventsBus.GetEventBodies(out _addRequestPool);
            _removeRequestFilter = _eventsBus.GetEventBodies(out _removeRequestPool);
            _linkPool = _world.GetPool<RegionLink>();
            _pool = _world.GetPool<RegionComponent>();
        }

        public void Create(List<RegionSavedData> regions, CellObject[] cells)
        {
            foreach (var region in regions)
            {
                var regionEntity = RegionFactoryTool.Create(_world, _pool, region.Type, region.CellsId.Count);

                foreach (var cellId in region.CellsId)
                    RegionAddCellTool.AddCell(cells[cellId].Entity, regionEntity, _linkPool, _pool);
            }
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