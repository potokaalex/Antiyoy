using System.Collections.Generic;
using Code.Gameplay.Cell;
using Code.Gameplay.Region.Tools;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace Code.Tests.Region
{
    public class RegionGetPartsTest
    {
        private EcsWorld _world;
        private EcsPool<CellComponent> _cellPool;
        
        [SetUp]
        public void Setup()
        {
            _world = new EcsWorld();
            _cellPool = _world.GetPool<CellComponent>();
        }
        
        [Test]
        public void WhenGettingRegionParts_AndRegionConsistsOf1Cell_ThenRegionPartsShouldBe1()
        {
            //Arrange.
            var cells = new List<int>
            {
                Create.CellWithNeighbour(_world)
            };

            //Act.
            var parts = RegionPartsTool.Get(cells, _cellPool);

            //Assert.
            parts.Count.Should().Be(1);
        }

        [Test]
        public void WhenGettingRegionParts_AndRegionConsistsOf3DependentCell_ThenCellCountInRegionPartShouldBe3()
        {
            //Arrange.
            var cells = new List<int>
            {
                Create.CellWithNeighbour(_world),
                Create.CellWithNeighbour(_world),
                Create.CellWithNeighbour(_world)
            };
            _cellPool.Get(cells[0]).NeighbourCellEntities.Add(cells[1]);
            _cellPool.Get(cells[1]).NeighbourCellEntities.Add(cells[2]);
            _cellPool.Get(cells[2]).NeighbourCellEntities.Add(cells[0]);

            //Act.
            var parts = RegionPartsTool.Get(cells, _cellPool);

            //Assert.
            parts[0].Cells.Count.Should().Be(3);
        }
        
        [Test]
        public void WhenGettingRegionParts_AndRegionConsistsOf3IndependentCell_ThenRegionPartsShouldBe3()
        {
            //Arrange.
            var cells = new List<int>
            {
                Create.CellWithNeighbour(_world),
                Create.CellWithNeighbour(_world),
                Create.CellWithNeighbour(_world)
            };

            //Act.
            var parts =RegionPartsTool.Get(cells, _cellPool);

            //Assert.
            parts.Count.Should().Be(3);
        }
    }
}