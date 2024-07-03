using System.Collections.Generic;
using Code.Cell;
using Code.Region.Components;
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

        public static int CellWithRegionLink(EcsWorld world, int regionEntity)
        {
            var cellEntity = Cell(world);
            world.GetPool<RegionLink>().Add(cellEntity).RegionEntity = regionEntity;
            return cellEntity;
        }
        
        public static IEcsProvider EcsProvider(EcsWorld world, EventsBus eventBus)
        {
            var ecsProvider = Substitute.For<IEcsProvider>();
            ecsProvider.GetWorld().Returns(world);
            ecsProvider.GetEventsBus().Returns(eventBus);
            return ecsProvider;
        }

        public static int Region(EcsWorld world)
        {
            var regionEntity = world.NewEntity();
            world.GetPool<RegionComponent>().Add(regionEntity).CellEntities = new List<int>();
            return regionEntity;
        }
    }
}