using System.Collections.Generic;
using Code.Cell;
using Code.Region;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace Code.Tests.Region
{
    public class RegionDivideToolTest
    {
        private EcsWorld _world;
        private EcsPool<CellComponent> _cellPool;
        private GetRegionPartsTool _getRegionPartsTool;
        private List<List<int>> _regionTilesParts;
        
        [SetUp]
        public void Setup()
        {
            _world = new EcsWorld();
            _cellPool = _world.GetPool<CellComponent>();
            _getRegionPartsTool = new GetRegionPartsTool(_cellPool);
            _regionTilesParts = new List<List<int>>();
        }
        
        [Test]
        public void WhenGetingRegionParts_AndRegionConsistsOf1Cell_ThenRegionPartsShouldBe1()
        {
            //Arrange.
            var regionTiles = new List<int>
            {
                Create.IndependentRegionCell(_world, _cellPool)
            };

            //Act.
            _getRegionPartsTool.Get(_regionTilesParts, regionTiles);

            //Assert.
            _regionTilesParts.Count.Should().Be(1);
        }

        [Test]
        public void WhenGetingRegionParts_AndRegionConsistsOf3DependentCell_ThenCellCountInRegionPartShouldBe3()
        {
            //Arrange.
            var regionTiles = new List<int>
            {
                Create.IndependentRegionCell(_world, _cellPool),
                Create.IndependentRegionCell(_world, _cellPool),
                Create.IndependentRegionCell(_world, _cellPool)
            };
            _cellPool.Get(regionTiles[0]).NeighboursCellsEntities.Add(regionTiles[1]);
            _cellPool.Get(regionTiles[1]).NeighboursCellsEntities.Add(regionTiles[2]);
            _cellPool.Get(regionTiles[2]).NeighboursCellsEntities.Add(regionTiles[0]);

            //Act.
            _getRegionPartsTool.Get(_regionTilesParts, regionTiles);

            //Assert.
            _regionTilesParts[0].Count.Should().Be(3);
        }
        
        [Test]
        public void WhenGetingRegionParts_AndRegionConsistsOf3IndependentCell_ThenRegionPartsShouldBe3()
        {
            //Arrange.
            var regionTiles = new List<int>
            {
                Create.IndependentRegionCell(_world, _cellPool),
                Create.IndependentRegionCell(_world, _cellPool),
                Create.IndependentRegionCell(_world, _cellPool)
            };

            //Act.
            _getRegionPartsTool.Get(_regionTilesParts, regionTiles);

            //Assert.
            _regionTilesParts.Count.Should().Be(3);
        }
    }
}