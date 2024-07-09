using Code.Gameplay.Cell;
using Code.Gameplay.Ecs;
using Code.Gameplay.Region.Components;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace Code.Gameplay.Region.Systems
{
    public class RegionDebugSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<CellComponent> _cellPool;
        private EcsFilter _cellFilter;
        private EcsPool<RegionLink> _linkPool;
        private EcsPool<RegionComponent> _pool;

        public RegionDebugSystem(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();

            _eventsBus = _ecsProvider.GetEventsBus();
            _cellFilter = world.Filter<CellComponent>().End();
            _cellPool = world.GetPool<CellComponent>();
            _linkPool = world.GetPool<RegionLink>();
            _pool = world.GetPool<RegionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!_eventsBus.HasEvents<RegionRemoveCellRequest>() && !_eventsBus.HasEvents<RegionAddCellRequest>())
                return;

            foreach (var cellEntity in _cellFilter)
            {
                if (_linkPool.Has(cellEntity))
                {
                    var link = _linkPool.Get(cellEntity);

                    _cellPool.Get(cellEntity).Object.DebugText.text =
                        $"{link.RegionEntity}\n{_pool.Get(link.RegionEntity).CellEntities.Count}".ToString();
                }
                else
                    _cellPool.Get(cellEntity).Object.DebugText.text = string.Empty;
            }
        }
    }
}