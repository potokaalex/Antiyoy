using System.Collections.Generic;
using Code.Region.Components;
using Code.Region.Systems;
using Code.Region.Tools;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace Code.Tests.Region
{
    public class RegionDivideTest
    {
        [Test]
        public void WhenDividingRegion_AndRegionHas3Parts_ThenRegionCountShouldBe3()
        {
            //Arrange.
            var world = new EcsWorld();
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
            RegionDivideTool.Divide(regionParts, baseRegionCells, world, pool, world.GetPool<RegionLink>());

            //Assert.
            var regionCount = world.Filter<RegionComponent>().End().GetEntitiesCount();
            regionCount.Should().Be(3);
        }
    }
}