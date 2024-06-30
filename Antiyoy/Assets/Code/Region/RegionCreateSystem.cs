using Code.Tile;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace Code.Region
{
    //теперь сложная часть: регионы!
    //регион эта сущнасть имеющая компонент регион.
    //компонент регион содержит количество тайлов в регионе.

    //вопрос о создании региона ставиться при создании тайла (или изменении владельца)
    //новый регион создаётся, если вокруг нет других регионов.
    //иначе, если рядом есть регион то мы ссылаеся на него.
    //если есть два и более подходящих региона, мы их объединяем и присоединяемся к уже новому объединённому региону
    public class RegionCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<TileCreateRequest> _tileRequestPool;
        private EcsPool<RegionComponent> _pool;
        private EcsFilter _tileRequestFilter;

        public RegionCreateSystem(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var world = _ecsProvider.GetWorld();
            
            _eventsBus = _ecsProvider.GetEventsBus();
            _tileRequestFilter = _eventsBus.GetEventBodies(out _tileRequestPool);
            _pool = world.GetPool<RegionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _tileRequestFilter)
            {
                var request = _tileRequestPool.Get(requestEntity);
                
                //как проверить, если ли вокруг другие регионы ?
                //смотрим на соседние тайлы.
                //соседние тайлы эт какие ? 
                //из request мы получаем клетку, из клетки мы можем вытащить индекс, из индекса получить индекс другой клетки и только теперь по индексу другой клетки отыскать нужную нам.
                //чтобы не делать это каждый раз, будем строить ссылки на соседние клетки при создании тайла!
                
            }
        }
    }
}