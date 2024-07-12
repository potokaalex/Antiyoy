using Code.Gameplay.Cell;
using Code.Gameplay.Ecs;
using Code.Gameplay.Region.Components;
using Code.Gameplay.Region.Tools;
using Leopotam.EcsLite;

namespace Code.Gameplay.Region.Systems
{
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