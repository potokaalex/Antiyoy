using System.Collections.Generic;
using Code.Cell;
using Code.Tile;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace Code.Region
{
    //1) добавляет регион
    //2) при создании тайла, возможна ситуация, при которой 2-3 региона соединяются.
    //как они соединяются?
    //мы смотрим на соседние регионы для новому тайлу, если мы находим 2 и более региона, находим в них major
    //переносим данные из других регионов в major и удаляем их
    public class RegionJoinSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private readonly List<int> _regionEntities = new();
        private EcsWorld _world;
        private EventsBus _eventsBus;
        private EcsFilter _tileRequestFilter;
        private EcsPool<TileCreateRequest> _tileRequestPool;
        private EcsPool<RegionComponent> _pool;
        private EcsPool<CellComponent> _cellPool;
        private EcsPool<RegionLink> _linkPool;

        public RegionJoinSystem(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var eventsBus = _ecsProvider.GetEventsBus();
            
            _world = _ecsProvider.GetWorld();
            _tileRequestFilter = eventsBus.GetEventBodies(out _tileRequestPool);
            _pool = _world.GetPool<RegionComponent>();
            _cellPool = _world.GetPool<CellComponent>();
            _linkPool = _world.GetPool<RegionLink>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _tileRequestFilter)
                Analyze(_tileRequestPool.Get(requestEntity));
        }

        private void Analyze(TileCreateRequest request)
        {
            var neighbours = _cellPool.Get(request.Cell.Entity).NeighboursCellsEntities;
            FullRegionEntities(neighbours);

            if (_regionEntities.Count == 0)
                CreateLinkAndConnect(request.Cell.Entity, CreateRegion());
            else if (_regionEntities.Count == 1)
                CreateLinkAndConnect(request.Cell.Entity, _regionEntities[0]);
            else
            {
                var majorRegionEntity = GetMajorRegion();
                ConcatenateRegions(majorRegionEntity);
                CreateLinkAndConnect(request.Cell.Entity, majorRegionEntity);
            }
        }

        //заполняет _regionEntities сущностями уникальных регионов которые располагаются по соседству.
        private void FullRegionEntities(List<int> neighbours)
        {
            _regionEntities.Clear();

            foreach (var neighbour in neighbours)
            {
                if (!_linkPool.Has(neighbour))
                    continue;

                var entity = _linkPool.Get(neighbour).Entity;

                if (!_regionEntities.Contains(entity))
                    _regionEntities.Add(entity);
            }
        }

        private int CreateRegion()
        {
            var entity = _world.NewEntity();
            ref var region = ref _pool.Add(entity);
            region.TilesEntities = ListPool<int>.Get();
            return entity;
        }

        private void CreateLinkAndConnect(int tileEntity, int regionEntity)
        {
            _linkPool.Add(tileEntity);
            ConnectTileWithRegion(tileEntity, regionEntity);
        }

        //перемещает данные из других регионов в major и удаляет другие регионы.
        private void ConcatenateRegions(int majorRegionEntity)
        {
            foreach (var regionEntity in _regionEntities)
            {
                if (regionEntity == majorRegionEntity)
                    continue;
                    
                var region = _pool.Get(regionEntity);

                foreach (var tile in region.TilesEntities)
                    ConnectTileWithRegion(tile, majorRegionEntity);

                ListPool<int>.Release(_pool.Get(regionEntity).TilesEntities);
                _pool.Del(regionEntity);
            }
        }

        private int GetMajorRegion()
        {
            var majorRegionEntity = _regionEntities[0];

            for (var i = 1; i < _regionEntities.Count; i++)
            {
                var mainRegion = _pool.Get(majorRegionEntity);
                var region = _pool.Get(_regionEntities[i]);

                if (region.TilesEntities.Count > mainRegion.TilesEntities.Count)
                    majorRegionEntity = _regionEntities[i];
            }

            return majorRegionEntity;
        }

        private void ConnectTileWithRegion(int tileEntity, int regionEntity)
        {
            ref var link = ref _linkPool.Get(tileEntity);
            link.Entity = regionEntity;

            ref var region = ref _pool.Get(regionEntity);
            region.TilesEntities.Add(tileEntity);
        }
    }
}