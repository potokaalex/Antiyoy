using System;
using System.Collections.Generic;
using Client.Code.Gameplay.Cell;
using Leopotam.EcsLite;
using Sirenix.Utilities;
using Zenject;

namespace Client.Code.Gameplay.Region
{
    public class RegionDivider : IInitializable
    {
        private readonly HashSet<int> _remaining = new();
        private readonly Stack<int> _front = new();
        private readonly EcsController _ecsController;
        private readonly RegionCreator _creator;
        private EcsPool<CellComponent> _cellPool;

        public RegionDivider(EcsController ecsController, RegionCreator creator)
        {
            _creator = creator;
            _ecsController = ecsController;
        }

        public void Initialize()
        {
            _cellPool = _ecsController.World.GetPool<CellComponent>();
        }

        //separates non-major regionParts from baseRegionCells. Creates new regions from non-major parts.
        public void Divide(RegionController region)
        {
            var regionParts = GetParts(region.CellEntities);

            if (regionParts.Count == 0 || regionParts[0].Cells.Count == region.CellEntities.Count)
            {
                ReleaseParts(regionParts);
                return;
            }

            var majorPart = GetMajorPart(regionParts);

            foreach (var part in regionParts)
            {
                if (part.Cells == majorPart.Cells)
                    continue;

                region.Remove(part.Cells);
                _creator.Create(part.Cells, region.Type);
            }

            ReleaseParts(regionParts);
        }

        private RegionPart GetMajorPart(List<RegionPart> parts)
        {
            var majorPart = parts[0];

            for (var i = 1; i < parts.Count; i++)
            {
                var part = parts[i];

                if (part.Cells.Count > majorPart.Cells.Count)
                    majorPart = part;
            }

            return majorPart;
        }

        private List<RegionPart> GetParts(List<int> baseRegionCells)
        {
            var resultParts = Core.ListPool<RegionPart>.Get();
            _remaining.AddRange(baseRegionCells);

            for (var i = 0; i < baseRegionCells.Count; i++)
            {
                var part = GetPart(_remaining);
                resultParts.Add(part);

                if (_remaining.Count == 0)
                    break;
            }

            if (_remaining.Count > 0)
                throw new Exception($"Error not all cells were passed: _remaining.Count = {_remaining.Count}!");

            return resultParts;
        }

        private void ReleaseParts(List<RegionPart> parts) //Use IDisposable
        {
            foreach (var tilesPart in parts) Core.ListPool<int>.Release(tilesPart.Cells);

            Core.ListPool<RegionPart>.Release(parts);
        }

        //passes the wave algorithm through noPassedCells and returns the cells that the algorithm has reached. Automatically remove cells from noPassedCells.
        private RegionPart GetPart(HashSet<int> noPassedCells)
        {
            var firstItem = 0;

            foreach (var c in noPassedCells)
            {
                firstItem = c;
                break;
            }

            var resultCells = Core.ListPool<int>.Get(noPassedCells.Count);
            var noPassedCellsInitialCount = noPassedCells.Count;

            _front.Push(firstItem);
            resultCells.Add(firstItem);
            noPassedCells.Remove(firstItem);

            for (var i = 0; i < noPassedCellsInitialCount + 1; i++)
            {
                var rootCell = _front.Pop();
                var neighbours = _cellPool.Get(rootCell).NeighbourCellEntities;

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