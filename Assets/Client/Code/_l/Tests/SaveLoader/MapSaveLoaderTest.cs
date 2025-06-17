using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;

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
            var isMapEmpty = _map.Size == new Vector2Int(0,0);
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
        public async Task WhenLoadMap_AndMapSavedWithKeyTestAndHasSizeOne_ThenMapWidthShouldBe1()
        {
            // Arrange.
            _saveLoader.Load(null);
            _map.Size = Vector2Int.one;
            await _saveLoader.Save(TestMapKey);

            // Act.
            _saveLoader.Load(TestMapKey);

            // Assert.
            _map.Size.Should().Be(Vector2Int.one);
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