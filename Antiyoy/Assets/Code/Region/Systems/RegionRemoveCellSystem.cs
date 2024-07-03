﻿using Code.Cell;
using Code.Region.Components;
using Code.Region.Tools;
using Code.Tile;
using Leopotam.EcsLite;

namespace Code.Region.Systems
{
    //1) удаляет регион
    //2) при удалении тайла, возможна ситуация, при которой разделяется регион на 2-3 части.
    //как проверить нужно ли разделить регион?
    //мы обходим все тайлы, если мы не можем попасть к следующему тайлу - создаём части регона.
    //выбираем из частей major, вычитаем из региона все не major части и создаём из них новые регионы.
    public class RegionRemoveCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private EcsWorld _world;
        private EcsPool<RegionRemoveCellRequest> _tileRequestPool;
        private EcsFilter _tileRequestFilter;
        private EcsPool<RegionComponent> _pool;
        private EcsPool<RegionLink> _linkPool;
        private EcsPool<CellComponent> _cellPool;

        public RegionRemoveCellSystem(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var eventsBus = _ecsProvider.GetEventsBus();

            _world = _ecsProvider.GetWorld();
            _tileRequestFilter = eventsBus.GetEventBodies(out _tileRequestPool);
            _pool = _world.GetPool<RegionComponent>();
            _linkPool = _world.GetPool<RegionLink>();
            _cellPool = _world.GetPool<CellComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _tileRequestFilter)
                Analyze(_tileRequestPool.Get(requestEntity));
        }

        private void Analyze(RegionRemoveCellRequest request)
        {
            var regionLink = _linkPool.Get(request.CellEntity);
            var baseRegion = _pool.Get(regionLink.RegionEntity);

            RemoveCell(request.CellEntity, baseRegion);

            if (baseRegion.CellEntities.Count == 0)
            {
                RegionFactoryTool.Destroy(regionLink.RegionEntity, _pool);
                return;
            }

            var regionParts = RegionPartsTool.Get(baseRegion.CellEntities, _cellPool);

            if (regionParts[0].Cells.Count != baseRegion.CellEntities.Count)
                RegionDivideTool.Divide(regionParts, baseRegion.CellEntities, _world, _pool, _linkPool);

            RegionPartsTool.Release(regionParts);
        }

        private void RemoveCell(int cellEntity, RegionComponent baseRegion)
        {
            _linkPool.Del(cellEntity);
            baseRegion.CellEntities.Remove(cellEntity);
        }
    }
}