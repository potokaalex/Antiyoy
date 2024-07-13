using System;
using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Region.Systems;
using ClientCode.Services;
using ClientCode.Utilities;
using Leopotam.EcsLite;

namespace ClientCode.Gameplay.Region.Tools
{
    public class RegionPartsTool
    {
        private static readonly HashSet<int> _remaining = new();
        private static readonly Stack<int> _front = new();

        public static List<RegionPart> Get(List<int> baseRegionCells, EcsPool<CellComponent> cellPool)
        {
            var resultParts = ListPool<RegionPart>.Get();

            for (var i = 0; i < baseRegionCells.Count; i++)
                _remaining.Add(baseRegionCells[i]);

            for (var i = 0; i < baseRegionCells.Count; i++)
            {
                var part = GetWavePart(_remaining, cellPool);
                resultParts.Add(part);

                if (_remaining.Count == 0)
                    break;
            }

            if (_remaining.Count > 0)
                throw new Exception($"Error not all cells were passed: _remaining.Count = {_remaining.Count}!");

            return resultParts;
        }

        public static void Release(List<RegionPart> parts)
        {
            foreach (var tilesPart in parts)
                ListPool<int>.Release(tilesPart.Cells);

            ListPool<RegionPart>.Release(parts);
        }

        //passes the wave algorithm through noPassedCells and returns the cells that the algorithm has reached. Automatically remove cells from noPassedCells.
        private static RegionPart GetWavePart(HashSet<int> noPassedCells, EcsPool<CellComponent> cellPool)
        {
            var firstItem = 0;

            foreach (var c in noPassedCells)
            {
                firstItem = c;
                break;
            }

            var resultCells = ListPool<int>.Get(noPassedCells.Count);
            var noPassedCellsInitialCount = noPassedCells.Count;

            _front.Push(firstItem);
            resultCells.Add(firstItem);
            noPassedCells.Remove(firstItem);

            for (var i = 0; i < noPassedCellsInitialCount + 1; i++)
            {
                var rootCell = _front.Pop();
                var neighbours = cellPool.Get(rootCell).NeighbourCellEntities;

                foreach (var neighbour in neighbours)
                {
                    if (!noPassedCells.Contains(neighbour))
                        continue;

                    resultCells.Add(neighbour);
                    _front.Push(neighbour);
                    noPassedCells.Remove(neighbour);
                }

                if (_front.Count == 0)
                    break;
            }

            if (_front.Count > 0)
                throw new Exception($"Error of the wave algorithm: _front.Count = {_front.Count}!");

            return new RegionPart { Cells = resultCells };
        }
    }
}