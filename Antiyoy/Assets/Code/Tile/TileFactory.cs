using Code.Cell;
using Leopotam.EcsLite;
using Plugins.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace Code.Tile
{
    //нажимая на клетку, я создаю тайл, тайл имеет регион и сущность.
    public class TileFactory
    {
        private readonly EcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileComponent> _pool;
        private EcsPool<TileCreateRequest> _createRequestPool;
        private EcsPool<TileDestroyRequest> _destroyRequestPool;
        private EcsFilter _createRequestFilter;
        private EcsFilter _destroyRequestFilter;

        public TileFactory(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

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
        }
        
        public void Destroy(CellObject cell)
        {
            if(_destroyRequestPool.Has(_destroyRequestFilter, r => r.Cell == cell))
                return;
            
            if (!_pool.Has(cell.Entity))
                return;

            ref var request = ref _eventsBus.NewEvent<TileDestroyRequest>();
            request.Cell = cell;

            //cell.SpriteRenderer.color = Color.black;
            //теперь сложная часть: регионы!
            //регион эта сущнасть имеющая компонент регион.
            //компонент регион содержит количество тайлов в регионе.

            //вопрос о создании региона ставиться при создании тайла (или изменении владельца)
            //новый регион создаётся, если вокруг нет других регионов.
            //иначе, если рядом есть регион то мы ссылаеся на него.
            //если есть два и более подходящих региона, мы их объединяем и присоединяемся к уже новому объединённому региону
        }
    }
}