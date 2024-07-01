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
        private EcsPool<CellComponent> _cellPool;
        private EcsPool<RegionLink> _linkPool;

        public RegionDivideSystem(EcsProvider ecsProvider) => _ecsProvider = ecsProvider;

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

        private void Analyze(TileDestroyRequest request)
        {
            var regionLink = _linkPool.Get(request.Cell.Entity);
            var baseRegion = _pool.Get(regionLink.Entity);

            RemoveTileFromRegion(request.Cell.Entity, baseRegion);

            if (baseRegion.TilesEntities.Count == 0)
            {
                _pool.Del(regionLink.Entity);
                return;
            }

            var regionTilesParts = ListPool<List<int>>.Get();
            GetRegionTilesParts(regionTilesParts, baseRegion, regionLink.Entity);

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
                newRegion.TilesEntities = new List<int>();

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

        //возвращает(regionTilesParts) тайлы из которых можно составить новые регионы (тайлы которые не граничат с другими тайлами)
        private void GetRegionTilesParts(List<List<int>> regionTilesParts, RegionComponent region, int baseRegionEntity)
        {
            var remainsTiles = ListPool<int>.Get(region.TilesEntities);

            for (var i = 0; i < region.TilesEntities.Count; i++)
            {
                var tiles = ListPool<int>.Get();
                GetWaveTiles(tiles, remainsTiles, baseRegionEntity);
                regionTilesParts.Add(tiles);

                foreach (var tile in tiles)
                    remainsTiles.Remove(tile);

                if (remainsTiles.Count == 0)
                    break;
            }

            ListPool<int>.Release(remainsTiles);
        }

        //проход волновым алгоритмом по regionTiles и возвращение(resultTiles) тайлов, до которых смог добраться алгоритм.
        private void GetWaveTiles(List<int> resultTiles, List<int> regionTiles, int baseRegionEntity)
        {
            var tilesFront = ListPool<int>.Get();
            tilesFront.Add(regionTiles[0]);
            resultTiles.Add(regionTiles[0]);

            for (var i = 0; i < regionTiles.Count; i++)
            {
                if (tilesFront.Count == 0)
                    break;

                var baseTile = tilesFront[0];
                var neighbours = _cellPool.Get(baseTile).NeighboursCellsEntities;

                foreach (var neighbour in neighbours)
                {
                    if (!_linkPool.Has(neighbour))
                        continue;

                    var link = _linkPool.Get(neighbour);

                    if (link.Entity != baseRegionEntity || resultTiles.Contains(neighbour))
                        continue;

                    resultTiles.Add(neighbour);
                    tilesFront.Add(neighbour);
                }

                tilesFront.Remove(baseTile);
            }

            ListPool<int>.Release(tilesFront);
        }
    }
}