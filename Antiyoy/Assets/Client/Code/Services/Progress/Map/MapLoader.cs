using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Tools;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.Progress.Actors;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Services.Progress.Map
{
    public class MapLoader : IProgressReader<MapProgressData>
    {
        private readonly CellFactory _cellFactory;
        private readonly IEcsProvider _ecsProvider;
        private EcsWorld _world;
        private EventsBus _eventBus;
        private EcsPool<RegionComponent> _regionPool;
        private EcsPool<RegionLink> _regionLinkPool;

        public MapLoader(CellFactory cellFactory, IEcsProvider ecsProvider)
        {
            _cellFactory = cellFactory;
            _ecsProvider = ecsProvider;
        }

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _eventBus = _ecsProvider.GetEventsBus();
            _regionPool = _world.GetPool<RegionComponent>();
            _regionLinkPool = _world.GetPool<RegionLink>();
        }

        public Task OnLoad(MapProgressData progress)
        {
            var cells = LoadCells(progress);
            LoadRegions(progress, cells);
            return Task.CompletedTask;
        }

        private CellObject[] LoadCells(MapProgressData data)
        {
            var cells = _cellFactory.Create(data.Width, data.Height);

            foreach (var tile in data.Tiles)
                _eventBus.NewEvent<TileCreateRequest>().Cell = cells[tile.Id];
            return cells;
        }

        private void LoadRegions(MapProgressData data, CellObject[] cells)
        {
            foreach (var region in data.Regions)
            {
                var regionEntity = RegionFactoryTool.Create(_world, _regionPool, region.CellsId.Count);

                foreach (var cellId in region.CellsId)
                    RegionAddCellTool.AddCell(cells[cellId].Entity, regionEntity, _regionLinkPool, _regionPool);
            }
        }
    }
}