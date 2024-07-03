using System.Collections.Generic;
using Code.Cell;
using Leopotam.EcsLite;

namespace Code.Tests.Region
{
    public class Create
    {
        public static int Cell(EcsWorld world, EcsPool<CellComponent> cellPool)
        {
            var cellEntity = world.NewEntity();
            ref var cell = ref cellPool.Add(cellEntity);
            cell.NeighbourCellEntities = new List<int>();
            return cellEntity;
        }

        public static int IndependentRegionCell(EcsWorld world, EcsPool<CellComponent> cellPool)
        {
            var cellEntity = Cell(world, cellPool);
            for (var i = 0; i < 6; i++)
                cellPool.Get(cellEntity).NeighbourCellEntities.Add(Cell(world, cellPool));
            return cellEntity;
        }
    }
}