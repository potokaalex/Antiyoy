using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Services.StaticDataProvider;
using ClientCode.UI.Windows;
using Leopotam.EcsLite;
using NSubstitute;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace Tests
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

        public static IWritingWindow WritingWindow(string getStringValue = "1")
        {
            var writingWindow = Substitute.For<IWritingWindow>();
            writingWindow.GetString().Returns(Task.FromResult(getStringValue));
            return writingWindow;
        }

        public static IStaticDataProvider StaticDataProvider()
        {
            var staticDataProvider = Substitute.For<IStaticDataProvider>();
            var cellObject = CellObject();
            staticDataProvider.Prefabs.CellObject = cellObject;
            return staticDataProvider;
        }

        public static CellFactory CellFactory(IEcsProvider ecsProvider, IStaticDataProvider staticDataProvider)
        {
            var cellFactory = new CellFactory(ecsProvider, staticDataProvider);
            cellFactory.Initialize();
            return cellFactory;
        }
        
        private static CellObject CellObject()
        {
            var cellObject = new GameObject().AddComponent<CellObject>();
            cellObject.SpriteRenderer = cellObject.gameObject.AddComponent<SpriteRenderer>();
            return cellObject;
        }
    }
}