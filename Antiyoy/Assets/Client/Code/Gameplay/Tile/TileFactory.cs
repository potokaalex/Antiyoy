using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Map.Factory;
using ClientCode.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Tile
{
    public class TileFactory : IProgressReader<MapProgressData>, IProgressWriter<MapProgressData>
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileComponent> _pool;
        private EcsPool<TileCreateRequest> _createRequestPool;
        private EcsPool<TileDestroyRequest> _destroyRequestPool;
        private EcsFilter _createRequestFilter;
        private EcsFilter _destroyRequestFilter;
        private MapProgressData _progress;
        private EcsFilter _filter;
        private EcsPool<CellComponent> _cellPool;

        public TileFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            var world = _ecsProvider.GetWorld();
            _eventsBus = _ecsProvider.GetEventsBus();
            _pool = world.GetPool<TileComponent>();
            _filter = world.Filter<TileComponent>().End();
            _createRequestFilter = _eventsBus.GetEventBodies(out _createRequestPool);
            _destroyRequestFilter = _eventsBus.GetEventBodies(out _destroyRequestPool);
            _cellPool = world.GetPool<CellComponent>();
        }

        public void Create(CellObject[] cells)
        {
            foreach (var tile in _progress.Tiles)
                _eventsBus.NewEvent<TileCreateRequest>().Cell = cells[tile.Id];
        }

        public void Create(CellObject cell)
        {
            if (_createRequestPool.Has(_createRequestFilter, r => r.Cell == cell))
                return;

            if (_pool.Has(cell.Entity))
                return;

            ref var request = ref _eventsBus.NewEvent<TileCreateRequest>();
            request.Cell = cell;
        }

        public void Destroy(CellObject cell)
        {
            if (_destroyRequestPool.Has(_destroyRequestFilter, r => r.Cell == cell))
                return;

            if (!_pool.Has(cell.Entity))
                return;

            ref var request = ref _eventsBus.NewEvent<TileDestroyRequest>();
            request.Cell = cell;
        }

        public void OnLoad(MapProgressData progress) => _progress = progress;

        public UniTask OnSave(MapProgressData progress)
        {
            progress.Tiles.Clear();

            foreach (var entity in _filter)
                progress.Tiles.Add(ProgressDataFactory.CreateTileData(entity, _cellPool));

            return UniTask.CompletedTask;
        }
    }
}