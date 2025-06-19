using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Systems
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

            DrawDebugText();
        }

        public void Run(IEcsSystems systems)
        {
            if (!_eventsBus.HasEvents<RegionRemoveCellRequest>() && !_eventsBus.HasEvents<RegionAddCellRequest>())
                return;

            DrawDebugText();
        }

        private void DrawDebugText()
        {
            foreach (var cellEntity in _cellFilter)
            {
                var cellDebug = _cellPool.Get(cellEntity).Debug;

                if (_linkPool.Has(cellEntity))
                {
                    var link = _linkPool.Get(cellEntity);
                    var region = _pool.Get(link.RegionEntity);
                    var text = $"{link.RegionEntity}\n{region.CellEntities.Count}";
                    SetText(cellDebug, text);
                }
                else
                    SetText(cellDebug, string.Empty);
            }
        }

        private static void SetText(CellDebugBehaviour cellDebug, string text)
        {
            if (cellDebug.Text.text != text)
                cellDebug.Text.SetText(text);
        }
    }
}