using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.SaveLoader
{
    public class MapSaveLoaderTest : IProgressReader<MapProgressData>
    {
        private const string TestMapKey = "Test";
        private MapProgressData _map;
        private MapSaveLoader _saveLoader;

        [SetUp]
        public void Setup()
        {
            _saveLoader = new MapSaveLoader();
            _saveLoader.RegisterActor(this);
        }

        [Test]
        public void WhenLoadMap_AndMapKeyIsNull_ThenMapShouldBeEmpty()
        {
            // Arrange.
            // Act.
            _saveLoader.Load(null);

            // Assert.
            var isMapEmpty = _map.Height == 0 && _map.Width == 0;
            isMapEmpty.Should().BeTrue();
        }

        [Test]
        public void WhenLoadMap_AndMapKeyIsNull_ThenResultShouldBeNormal()
        {
            // Arrange.
            // Act.
            var result = _saveLoader.Load(null);

            // Assert.
            result.Should().Be(SaveLoaderResultType.Normal);
        }

        [Test]
        public async Task WhenLoadMap_AndMapSavedWithKeyTestAndHasWidth1_ThenMapWidthShouldBe1()
        {
            // Arrange.
            _saveLoader.Load(null);
            _map.Width = 1;
            await _saveLoader.Save(TestMapKey);

            // Act.
            _saveLoader.Load(TestMapKey);

            // Assert.
            _map.Width.Should().Be(1);
        }

        [Test]
        public async Task WhenSaveMap_AndMapLoaded_ThenResultShouldBeNormal()
        {
            //Arrange.
            _saveLoader.Load(TestMapKey, new MapProgressData());

            //Act.
            var result = await _saveLoader.Save(TestMapKey);

            //Assert.
            result.Should().Be(SaveLoaderResultType.Normal);
        }

        [TearDown]
        public void TearDown()
        {
            _saveLoader.UnRegisterActor(this);
            _saveLoader.Remove(TestMapKey);
        }

        public void OnLoad(MapProgressData progress) => _map = progress;
    }
}