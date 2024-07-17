using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Data.Saved;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.Progress.Actors;
using Leopotam.EcsLite;

namespace ClientCode.Services.Progress.Map.Save
{
    public class MapSaver : IProgressWriter<MapProgressData>
    {
        private readonly MapDataFactory _dataFactory;
        private readonly IEcsProvider _ecsProvider;
        private EcsFilter _tileFilter;
        private EcsFilter _regionFiler;

        public MapSaver(MapDataFactory dataFactory, IEcsProvider ecsProvider)
        {
            _dataFactory = dataFactory;
            _ecsProvider = ecsProvider;
        }

        public void Initialize()
        {
            var world = _ecsProvider.GetWorld();
            _tileFilter = world.Filter<TileComponent>().End();
            _regionFiler = world.Filter<RegionComponent>().End();
        }

        public Task OnSave(MapProgressData progress)
        {
            SaveCells(progress);
            SaveRegions(progress);
            return Task.CompletedTask;
        }

        private void SaveCells(MapProgressData data)
        {
            data.Tiles = new List<TileSavedData>();

            foreach (var entity in _tileFilter)
                data.Tiles.Add(_dataFactory.CreateTileData(entity, data.Height));
        }

        private void SaveRegions(MapProgressData data)
        {
            data.Regions = new List<RegionSavedData>();
            foreach (var entity in _regionFiler)
                data.Regions.Add(_dataFactory.CreateRegionData(entity, data.Height));
        }
    }
}