using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Tools;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Systems
{
    public class RegionAddCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private readonly List<int> _neighbourRegions = new();
        private EcsWorld _world;
        private EcsFilter _requestFilter;
        private EcsPool<RegionAddCellRequest> _requestPool;
        private EcsPool<RegionComponent> _pool;
        private EcsPool<CellComponent> _cellPool;
        private EcsPool<RegionLink> _linkPool;
        private EventsBus _events;

        public RegionAddCellSystem(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            _world = _ecsProvider.GetWorld();
            _events = _ecsProvider.GetEventsBus();
            _requestFilter = _events.GetEventBodies(out _requestPool);
            _pool = _world.GetPool<RegionComponent>();
            _cellPool = _world.GetPool<CellComponent>();
            _linkPool = _world.GetPool<RegionLink>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _requestFilter)
                Analyze(_requestPool.Get(requestEntity));
        }

        private void Analyze(RegionAddCellRequest request)
        {
            FillNeighboursRegionsBuffer(request.CellEntity, request.Type);

            int regionEntity;

            if (_neighbourRegions.Count == 0)
                regionEntity = RegionFactoryTool.Create(_world, _pool, request.Type, _events);
            else if (_neighbourRegions.Count == 1)
                regionEntity = _neighbourRegions[0];
            else
                regionEntity = RegionJoinTool.Join(_neighbourRegions, _pool, _linkPool, _events);

            RegionAddCellTool.AddCell(request.CellEntity, regionEntity, _linkPool, _pool);
        }

        //заполняет _regionEntities сущностями уникальных регионов которые располагаются по соседству.
        private void FillNeighboursRegionsBuffer(int regionEntity, RegionType regionType)
        {
            var neighbours = _cellPool.Get(regionEntity).NeighbourCellEntities;
            _neighbourRegions.Clear();

            foreach (var neighbour in neighbours)
            {
                if (!_linkPool.Has(neighbour))
                    continue;

                var entity = _linkPool.Get(neighbour).RegionEntity;

                if (_pool.Get(entity).Type != regionType)
                    continue;

                if (!_neighbourRegions.Contains(entity))
                    _neighbourRegions.Add(entity);
            }
        }
    }
}