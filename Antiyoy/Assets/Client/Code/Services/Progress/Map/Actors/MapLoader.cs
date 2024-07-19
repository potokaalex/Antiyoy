using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Tools;
using ClientCode.Gameplay.Tile;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.Progress.Actors;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Services.Progress.Map.Actors
{
    public class MapLoader : IProgressReader<MapProgressData>
    {
        private readonly CellFactory _cellFactory;
        private readonly TileFactory _tileFactory;
        private readonly RegionFactory _regionFactory;
        private readonly IEcsProvider _ecsProvider;
        private EcsWorld _world;
        private EventsBus _eventBus;
        private EcsPool<RegionComponent> _regionPool;
        private EcsPool<RegionLink> _regionLinkPool;
        private MapProgressData _progress;

        public MapLoader(CellFactory cellFactory, TileFactory tileFactory, RegionFactory regionFactory, IEcsProvider ecsProvider)
        {
            _cellFactory = cellFactory;
            _tileFactory = tileFactory;
            _regionFactory = regionFactory;
            _ecsProvider = ecsProvider;
        }

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _eventBus = _ecsProvider.GetEventsBus();
            _regionPool = _world.GetPool<RegionComponent>();
            _regionLinkPool = _world.GetPool<RegionLink>();

            var cells = LoadCells(_progress);
            _regionFactory.Create(_progress.Regions, cells);
        }

        public void OnLoad(MapProgressData progress) => _progress = progress;

        private CellObject[] LoadCells(MapProgressData data)
        {
            var cells = _cellFactory.Create(data.Width, data.Height);
            _tileFactory.Create(data.Tiles, cells);
            return cells;
        }
    }
}