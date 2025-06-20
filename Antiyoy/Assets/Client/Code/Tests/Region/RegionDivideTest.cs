using System.Collections.Generic;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Systems;
using ClientCode.Gameplay.Region.Tools;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using SevenBoldPencil.EasyEvents;

namespace Tests.Region
{
    public class RegionDivideTest
    {
        [Test]
        public void WhenDividingRegion_AndRegionHas3Parts_ThenRegionCountShouldBe3()
        {
            //Arrange.
            var world = new EcsWorld();
            var events = new EventsBus();
            var regionEntity = world.NewEntity();
            var pool = world.GetPool<RegionComponent>();

            pool.Add(regionEntity);

            var baseRegionCells = new List<int>();
            for (var i = 0; i < 4; i++)
                baseRegionCells.Add(Create.CellWithRegionLink(world, regionEntity));

            var regionParts = new List<RegionPart>()
            {
                new() { Cells = new List<int> { baseRegionCells[0], baseRegionCells[1] } },
                new() { Cells = new List<int> { baseRegionCells[2] } },
                new() { Cells = new List<int> { baseRegionCells[3] } }
            };

            //Act.
            RegionDivideTool.Divide(regionParts, baseRegionCells, world, pool, world.GetPool<RegionLink>(), 0, events);

            //Assert.
            var regionCount = world.Filter<RegionComponent>().End().GetEntitiesCount();
            regionCount.Should().Be(3);
        }
    }
}