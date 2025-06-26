using Client.Code.Gameplay.Cell;
using Leopotam.EcsLite;

namespace Client.Code.Gameplay.Region
{
    public class RegionDebugSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CellComponent> _cellPool;
        private EcsFilter _cellFilter;
        private EcsPool<RegionLink> _linkPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _cellFilter = world.Filter<CellComponent>().End();
            _cellPool = world.GetPool<CellComponent>();
            _linkPool = world.GetPool<RegionLink>();
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