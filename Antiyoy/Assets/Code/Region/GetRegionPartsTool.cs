using System;
using System.Collections.Generic;
using Code.Cell;
using Leopotam.EcsLite;

namespace Code.Region
{
    public class GetRegionPartsTool
    {
        private readonly EcsPool<CellComponent> _cellPool;
        private readonly HashSet<int> _remaining = new();
        private readonly Stack<int> _tilesFront = new();

        public GetRegionPartsTool(EcsPool<CellComponent> cellPool) => _cellPool = cellPool;

        //возвращает(regionTilesParts) тайлы из которых можно составить новые регионы (тайлы которые не граничат с другими тайлами)
        public void Get(List<List<int>> regionTilesParts, RegionComponent region)
        {
            for (var i = 0; i < region.TilesEntities.Count; i++)
                _remaining.Add(region.TilesEntities[i]);

            for (var i = 0; i < region.TilesEntities.Count; i++)
            {
                var tiles = GetWaveTiles(_remaining);
                regionTilesParts.Add(tiles);

                if (_remaining.Count == 0)
                    break;
            }
            
            if(_remaining.Count  > 0)
                throw new Exception($"Error not all tiles were passed: _remaining.Count = {_remaining.Count}!");
        }

        //проход волновым алгоритмом по regionTiles и возвращение(resultTiles) тайлов, до которых смог добраться алгоритм.
        private List<int> GetWaveTiles(HashSet<int> noPassedTiles)
        {
            var firstItem = 0;

            foreach (var tile in noPassedTiles)
            {
                firstItem = tile;
                break;
            }
            
            var resultTiles = ListPool<int>.Get(noPassedTiles.Count);
            var noPassedTilesInitialCount = noPassedTiles.Count;
            
            _tilesFront.Push(firstItem);
            resultTiles.Add(firstItem);
            noPassedTiles.Remove(firstItem);

            for (var i = 0; i < noPassedTilesInitialCount + 1; i++)
            {
                var baseTile = _tilesFront.Pop();
                var neighbours = _cellPool.Get(baseTile).NeighboursCellsEntities;

                foreach (var neighbour in neighbours)
                {
                    if (!noPassedTiles.Contains(neighbour))
                        continue;

                    resultTiles.Add(neighbour);
                    _tilesFront.Push(neighbour);
                    noPassedTiles.Remove(neighbour);
                }

                if (_tilesFront.Count == 0)
                    break;
            }

            if (_tilesFront.Count > 0)
                throw new Exception($"Error of the wave algorithm: _tilesFront.Count = {_tilesFront.Count}!");

            _tilesFront.Clear();
            return resultTiles;
        }
    }
}