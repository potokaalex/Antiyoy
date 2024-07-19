using System.Collections.Generic;
using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Tile.Components;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Map.Actors;
using ClientCode.Services.Progress.Map.Factory;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using SevenBoldPencil.EasyEvents;

namespace Tests.SaveLoader
{
    public class MapSaverTest
    {
        [Test]
        public void WhenMapSaverOnSave_AndWorldHas1TileWith1Region_ThenMapShouldHas1TileWith1Region()
        {
            //Arrange.
            var world = new EcsWorld();
            var eventBus = new EventsBus();
            var ecsProvider = Create.EcsProvider(world, eventBus);

            var region = Create.Region(world);
            var cell = Create.CellWithRegionLink(world, region);
            world.GetPool<TileComponent>().Add(cell);
            world.GetPool<RegionComponent>().Get(region).CellEntities = new List<int> { cell };

            var mapDataFactory = new MapDataFactory(ecsProvider);
            mapDataFactory.Initialize();

            var mapSaver = new MapSaver(mapDataFactory, ecsProvider);
            var map = new MapProgressData();

            //Act.
            mapSaver.Initialize();
            mapSaver.OnSave(map);

            //Assert.
            var regionCell = map.Regions[0].CellsId[0];
            var tile = map.Tiles[0].Id;
            regionCell.Should().Be(tile);
        }
    }
}