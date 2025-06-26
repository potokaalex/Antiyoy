using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Systems
{
    public class RegionDebugSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CellComponent> _cellPool;
        private EcsFilter _cellFilter;
        private EcsPool<Infrastructure.Installers.RegionLink> _linkPool;
        private EcsPool<RegionComponent> _pool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _cellFilter = world.Filter<CellComponent>().End();
            _cellPool = world.GetPool<CellComponent>();
            _linkPool = world.GetPool<Infrastructure.Installers.RegionLink>();
            _pool = world.GetPool<RegionComponent>();
            DrawDebugText();
        }

        public void Run(IEcsSystems systems) => DrawDebugText();

        private void DrawDebugText()
        {
            foreach (var cellEntity in _cellFilter)
            {
                var cellDebug = _cellPool.Get(cellEntity).Debug;

                if (_linkPool.Has(cellEntity))
                {
                    var region = _linkPool.Get(cellEntity).Region;
                    var text = $"{region.CellEntities.Count}";
                    SetText(cellDebug, text);
                }
                else
                    SetText(cellDebug, string.Empty);
            }
        }

        private void SetText(CellDebugBehaviour cellDebug, string text)
        {
            if (cellDebug.Text.text != text)
                cellDebug.Text.SetText(text);
        }
    }
}