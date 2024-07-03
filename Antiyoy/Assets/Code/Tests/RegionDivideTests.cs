using System.Collections.Generic;
using Code.Cell;
using Code.Region;
using FluentAssertions;
using Leopotam.EcsLite;
using NSubstitute;
using NUnit.Framework;

namespace Code.Tests
{
    public class RegionDivideTests
    {
        //будующие тесты:
        //регион состоит из 1го тайла и разделён на 1 часть
        //регион состоит из 3х тайлов и разделён на 3 части 

        [Test]
        public void WhenGetingRegionParts_AndRegionConsistsOf1Tile_ThenRegionPartsShouldBe1()
        {
            // Arrange.
            var world = new EcsWorld();
            var cellPool = world.GetPool<CellComponent>();
            
            var cellEntity = CreateCell(world, cellPool);
            
            for (var i = 0; i < 6; i++) //заполняю 6 соседей для клетки.
                cellPool.Get(cellEntity).NeighboursCellsEntities.Add(CreateCell(world, cellPool));
            
            var getRegionPartsTool = new GetRegionPartsTool(cellPool);
            var regionTilesParts = new List<List<int>>();
            var regionTiles = new List<int> { cellEntity };

            // Act.
            getRegionPartsTool.Get(regionTilesParts, regionTiles);

            // Assert.
            regionTilesParts.Count.Should().Be(1);
        }

        private int CreateCell(EcsWorld world, EcsPool<CellComponent> cellPool)
        {
            var cellEntity = world.NewEntity();
            ref var cell = ref cellPool.Add(cellEntity);
            cell.NeighboursCellsEntities = new List<int>();
            return cellEntity;
        }
    }
}