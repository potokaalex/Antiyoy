using System.Collections.Generic;
using Code.Cell;
using Leopotam.EcsLite;
using NSubstitute;
using SevenBoldPencil.EasyEvents;

namespace Code.Tests.Region
{
    public class Create
    {
        public static int Cell(EcsWorld world)
        {
            var pool = world.GetPool<CellComponent>();
            var cellEntity = world.NewEntity();
            ref var cell = ref pool.Add(cellEntity);
            cell.NeighbourCellEntities = new List<int>();
            return cellEntity;
        }

        public static int CellWithNeighbour(EcsWorld world)
        {
            var cellEntity = Cell(world);
            var pool = world.GetPool<CellComponent>();
            
            for (var i = 0; i < 6; i++)
                pool.Get(cellEntity).NeighbourCellEntities.Add(Cell(world));
            
            return cellEntity;
        }

        public static IEcsProvider EcsProvider(EcsWorld world, EventsBus eventBus)
        {
            var ecsProvider = Substitute.For<IEcsProvider>();
            ecsProvider.GetWorld().Returns(world);
            ecsProvider.GetEventsBus().Returns(eventBus);
            return ecsProvider;
        }
    }
}