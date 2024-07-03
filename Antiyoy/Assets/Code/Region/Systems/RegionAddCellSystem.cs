using System.Collections.Generic;
using Code.Cell;
using Code.Region.Components;
using Code.Region.Tools;
using Leopotam.EcsLite;

namespace Code.Region.Systems
{
    //1) добавляет регион
    //2) при создании тайла, возможна ситуация, при которой 2-3 региона соединяются.
    //как они соединяются?
    //мы смотрим на соседние регионы для новому тайлу, если мы находим 2 и более региона, находим в них major
    //переносим данные из других регионов в major и удаляем их
    public class RegionAddCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private readonly List<int> _neighbourRegions = new();
        private EcsWorld _world;
        private EcsFilter _requestFilter;
        private EcsPool<RegionAddCellRequest> _requestPool;
        private EcsPool<RegionComponent> _pool;
        private EcsPool<CellComponent> _cellPool;
        private EcsPool<RegionLink> _linkPool;
        
        public RegionAddCellSystem(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var eventsBus = _ecsProvider.GetEventsBus();
            
            _world = _ecsProvider.GetWorld();
            _requestFilter = eventsBus.GetEventBodies(out _requestPool);
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
            FillNeighboursRegionsBuffer(request.CellEntity);

            //ReSharper disable once RedundantAssignment
            var regionEntity = -1;

            if (_neighbourRegions.Count == 0)
                regionEntity = RegionFactoryTool.Create(_world, _pool);
            else if (_neighbourRegions.Count == 1)
                regionEntity = _neighbourRegions[0];
            else
                regionEntity = RegionJoinTool.Join(_neighbourRegions, _pool, _linkPool);
            
            RegionAddCellTool.AddCell(request.CellEntity, regionEntity, _linkPool, _pool);
        }

        //заполняет _regionEntities сущностями уникальных регионов которые располагаются по соседству.
        private void FillNeighboursRegionsBuffer(int regionEntity)
        {
            var neighbours = _cellPool.Get(regionEntity).NeighbourCellEntities;
            _neighbourRegions.Clear();

            foreach (var neighbour in neighbours)
            {
                if (!_linkPool.Has(neighbour))
                    continue;

                var entity = _linkPool.Get(neighbour).RegionEntity;

                if (!_neighbourRegions.Contains(entity))
                    _neighbourRegions.Add(entity);
            }
        }
    }
}