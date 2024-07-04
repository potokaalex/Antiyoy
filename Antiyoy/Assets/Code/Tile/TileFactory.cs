using Code.Cell;
using Code.Ecs;
using Code.Region.Components;
using Code.Tile.Components;
using Leopotam.EcsLite;
using Plugins.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace Code.Tile
{
    public class TileFactory
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileComponent> _pool;
        private EcsPool<TileCreateRequest> _createRequestPool;
        private EcsPool<TileDestroyRequest> _destroyRequestPool;
        private EcsFilter _createRequestFilter;
        private EcsFilter _destroyRequestFilter;

        public TileFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            var world = _ecsProvider.GetWorld();

            _eventsBus = _ecsProvider.GetEventsBus();
            _pool = world.GetPool<TileComponent>();
            _createRequestFilter = _eventsBus.GetEventBodies(out _createRequestPool);
            _destroyRequestFilter = _eventsBus.GetEventBodies(out _destroyRequestPool);
        }
        
        public void Create(CellObject cell)
        {
            if(_createRequestPool.Has(_createRequestFilter, r => r.Cell == cell))
                return;
            
            if (_pool.Has(cell.Entity))
                return;

            ref var request = ref _eventsBus.NewEvent<TileCreateRequest>();
            request.Cell = cell;
            
            ref var regionRequest = ref _eventsBus.NewEvent<RegionAddCellRequest>();
            regionRequest.CellEntity = cell.Entity;
        }
        
        public void Destroy(CellObject cell)
        {
            if(_destroyRequestPool.Has(_destroyRequestFilter, r => r.Cell == cell))
                return;
            
            if (!_pool.Has(cell.Entity))
                return;

            ref var request = ref _eventsBus.NewEvent<TileDestroyRequest>();
            request.Cell = cell;
            
            ref var regionRequest = ref _eventsBus.NewEvent<RegionRemoveCellRequest>();
            regionRequest.CellEntity = cell.Entity;
        }
    }
}