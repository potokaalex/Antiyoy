using Code.Gameplay.Ecs;
using Code.Gameplay.Region.Components;
using Code.Gameplay.Region.Systems;
using FluentAssertions;
using Leopotam.EcsLite;
using NUnit.Framework;
using SevenBoldPencil.EasyEvents;

namespace Code.Tests.Region
{
    public class RegionAddCellTest
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
        public void WhenAddingRegion_AndNoRegions_ThenRegionCountShouldBe1()
        {
            //Arrange.
            ref var request = ref _eventBus.NewEvent<RegionAddCellRequest>();
            request.CellEntity = Create.Cell(_world);
            _systems.Add(new RegionAddCellSystem(_ecsProvider));
            
            //Act.
            _systems.Init();
            _systems.Run();
            
            //Assert.
            var regionCount = _world.Filter<RegionComponent>().End().GetEntitiesCount();
            regionCount.Should().Be(1);
        }
        
        [Test]
        public void WhenAddingRegion_AndNoRegions_ThenCellShouldHasRegionWithCellCount1()
        {
            //Arrange.
            ref var request = ref _eventBus.NewEvent<RegionAddCellRequest>();
            request.CellEntity = Create.Cell(_world);
            _systems.Add(new RegionAddCellSystem(_ecsProvider));
            
            //Act.
            _systems.Init();
            _systems.Run();
            
            //Assert.
            var regionEntity = _world.GetPool<RegionLink>().Get(request.CellEntity).RegionEntity;
            var regionCellCount = _world.GetPool<RegionComponent>().Get(regionEntity).CellEntities.Count;
            regionCellCount.Should().Be(1);
        }
    }
}