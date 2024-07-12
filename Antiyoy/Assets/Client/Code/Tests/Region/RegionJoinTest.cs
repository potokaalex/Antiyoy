using System.Collections.Generic;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Tools;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace Tests.Region
{
    public class RegionJoinTest
    {
        [Test]
        public void WhenJoiningRegions_AndRegionsCountIs3_ThenRegionCountShouldBe1()
        {
            //Arrange.
            var world = new EcsWorld();
            var pool = world.GetPool<RegionComponent>();
            var linkPool = world.GetPool<RegionLink>();
            var regions = new List<int>
            {
                Create.Region(world),
                Create.Region(world),
                Create.Region(world)
            };
            
            //Act.
            RegionJoinTool.Join(regions, pool, linkPool);
            
            //Assert.
            var regionCount = world.Filter<RegionComponent>().End().GetEntitiesCount();
            regionCount.Should().Be(1);
        }
    }
}