using System.Collections.Generic;
using ClientCode.Data.Progress.Map;
using ClientCode.Data.Saved;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Gameplay.Tile.Systems;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Map.Actors;
using ClientCode.Utilities.Extensions;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using SevenBoldPencil.EasyEvents;

namespace Tests.SaveLoader
{
    public class MapLoaderTest
    {
        [Test]
        public void WhenMapLoaderOnLoad_AndMapHas1TileWith1Region_ThenWorldShouldHas1TileWith1Region()
        {
            //Arrange.
            var world = new EcsWorld();
            var eventBus = new EventsBus();
            var ecsProvider = Create.EcsProvider(world, eventBus);
            
            var staticDataProvider = Create.StaticDataProvider();
            var cellFactory = Create.CellFactory(ecsProvider, staticDataProvider);
            var mapLoader = new MapLoader(cellFactory, ecsProvider);
            var map = new MapProgressData
            {
                Width = 10,
                Height = 10,
                Tiles = new List<TileSavedData> { new() { Id = 1 } },
                Regions = new List<RegionSavedData> { new() { CellsId = new List<int> { 1 } } }
            };

            var systems = new EcsSystems(world);
            systems.Add(new TileCreateSystem(ecsProvider));
            systems.Init();
            
            //Act.
            mapLoader.OnLoad(map);
            mapLoader.Initialize();
            systems.Run();
            
            //Assert.
            var tile = world.Filter<TileComponent>().End().GetFirstOrDefault();
            var region = world.GetPool<RegionLink>().Get(tile).RegionEntity;
            var tileInRegion = world.GetPool<RegionComponent>().Get(region).CellEntities[0];
            tileInRegion.Should().Be(tile);
        }
    }
}