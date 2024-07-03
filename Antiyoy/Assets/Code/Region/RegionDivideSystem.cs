using System.Collections.Generic;
using Code.Cell;
using Code.Tile;
using Leopotam.EcsLite;

namespace Code.Region
{
    //1) удаляет регион
    //2) при удалении тайла, возможна ситуация, при которой разделяется регион на 2-3 части.
    //как проверить нужно ли разделить регион?
    //мы обходим все тайлы, если мы не можем попасть к следующему тайлу - создаём части регона.
    //выбираем из частей major, вычитаем из региона все не major части и создаём из них новые регионы.
    public class RegionDivideSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsProvider _ecsProvider;
        private EcsWorld _world;
        private EcsPool<TileDestroyRequest> _tileRequestPool;
        private EcsFilter _tileRequestFilter;
        private EcsPool<RegionComponent> _pool;
        private EcsPool<RegionLink> _linkPool;
        private GetRegionPartsTool _getRegionPartsTool;

        public RegionDivideSystem(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Init(IEcsSystems systems)
        {
            var eventsBus = _ecsProvider.GetEventsBus();

            _world = _ecsProvider.GetWorld();
            _tileRequestFilter = eventsBus.GetEventBodies(out _tileRequestPool);
            _pool = _world.GetPool<RegionComponent>();
            _linkPool = _world.GetPool<RegionLink>();
            var cellPool = _world.GetPool<CellComponent>();
            _getRegionPartsTool = new GetRegionPartsTool(cellPool);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _tileRequestFilter)
                Analyze(_tileRequestPool.Get(requestEntity));
        }

        private void Analyze(TileDestroyRequest request)
        {
            var regionLink = _linkPool.Get(request.Cell.Entity);
            var baseRegion = _pool.Get(regionLink.Entity);

            RemoveTileFromRegion(request.Cell.Entity, baseRegion);

            if (baseRegion.TilesEntities.Count == 0)
            {
                ListPool<int>.Release(_pool.Get(regionLink.Entity).TilesEntities);
                _pool.Del(regionLink.Entity);
                return;
            }

            var regionTilesParts = ListPool<List<int>>.Get();
            _getRegionPartsTool.Get(regionTilesParts, baseRegion.TilesEntities);

            if (regionTilesParts[0].Count != baseRegion.TilesEntities.Count)
                Divide(regionTilesParts, baseRegion);

            foreach (var tilesPart in regionTilesParts)
                ListPool<int>.Release(tilesPart);
            ListPool<List<int>>.Release(regionTilesParts);
        }

        private void RemoveTileFromRegion(int tileEntity, RegionComponent baseRegion)
        {
            _linkPool.Del(tileEntity);
            baseRegion.TilesEntities.Remove(tileEntity);
        }

        //отделяем от major региона тайлы и создаёт из них новые регионы
        private void Divide(List<List<int>> regionTilesParts, RegionComponent baseRegion)
        {
            var majorTilesPart = GetMajorTilesPart(regionTilesParts);

            foreach (var tilesPart in regionTilesParts)
            {
                if (tilesPart == majorTilesPart)
                    continue;

                var newRegionEntity = _world.NewEntity();
                ref var newRegion = ref _pool.Add(newRegionEntity);
                newRegion.TilesEntities = ListPool<int>.Get(tilesPart.Count);

                MoveTiles(tilesPart, newRegion, baseRegion, newRegionEntity);
            }
        }

        private void MoveTiles(List<int> tilesPart, RegionComponent newRegion, RegionComponent baseRegion,
            int newRegionEntity)
        {
            foreach (var tileEntity in tilesPart)
            {
                newRegion.TilesEntities.Add(tileEntity);
                baseRegion.TilesEntities.Remove(tileEntity);

                ref var link = ref _linkPool.Get(tileEntity);
                link.Entity = newRegionEntity;
            }
        }

        private static List<int> GetMajorTilesPart(List<List<int>> regionTilesParts)
        {
            var majorTilesPart = regionTilesParts[0];

            for (var i = 1; i < regionTilesParts.Count; i++)
            {
                var tilesPart = regionTilesParts[i];

                if (tilesPart.Count > majorTilesPart.Count)
                    majorTilesPart = tilesPart;
            }

            return majorTilesPart;
        }
    }
}