using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Gameplay.Region.Systems;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using SevenBoldPencil.EasyEvents;

namespace Tests.Region
{
    public class RegionRemoveCellTest
    {
        private EcsWorld _world;
        private EventsBus _eventBus;
        private EcsSystems _systems;
        private IEcsProvider _ecsProvider;

        [SetUp]
        public void Setup()
        {
            _world = new EcsWorld();
            _eventBus = new EventsBus();
            _systems = new EcsSystems(_world);
            _ecsProvider = Create.EcsProvider(_world, _eventBus);
        }

        [Test]
        public void WhenRemovingRegion_AndThereIs1CellWithRegion_ThenRegionCountShouldBe0()
        {
            //Arrange.
            var regionEntity = Create.Region(_world);
            var cellEntity = Create.CellWithRegionLink(_world, regionEntity);
            _systems.Add(new RegionRemoveCellSystem(_ecsProvider));
            _eventBus.NewEvent<RegionRemoveCellRequest>().CellEntity = cellEntity;

            //Act.
            _systems.Init();
            _systems.Run();

            //Assert.
            var regionCount = _world.Filter<RegionComponent>().End().GetEntitiesCount();
            regionCount.Should().Be(0);
        }

        [Test]
        public void WhenRemovingRegion_AndThereIs1CellWithRegion_ThenCellShouldHasNoRegion()
        {
            //Arrange.
            var regionEntity = Create.Region(_world);
            var cellEntity = Create.CellWithRegionLink(_world, regionEntity);
            _systems.Add(new RegionRemoveCellSystem(_ecsProvider));
            _eventBus.NewEvent<RegionRemoveCellRequest>().CellEntity = cellEntity;

            //Act.
            _systems.Init();
            _systems.Run();

            //Assert.
            var isCellHasRegion = _world.GetPool<RegionLink>().Has(cellEntity);
            isCellHasRegion.Should().BeFalse();
        }
    }
}