using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Map.Save;
using ClientCode.UI.Factory;
using ClientCode.UI.Windows.Base;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.SaveLoader
{
    public class MapKeySaverTest
    {
        [Test]
        public async Task WhenOnSave_AndMapKeySaverWritesKey1_ThenMapKeyShouldBe1()
        {
            //Arrange.
            var map = new MapProgressData();
            var logReceiver = new LogReceiver();
            var saveLoader = new MapSaveLoader();

            var writingWindow = Create.WritingWindow();
            var windowsFactory = Substitute.For<IWindowsFactory>();
            windowsFactory.Get(WindowType.Writing).Returns(writingWindow);

            var mapKeySaver = new MapKeySaver(windowsFactory, saveLoader, logReceiver);

            //Act.
            await mapKeySaver.OnSave(map);

            //Assert.
            map.Key.Should().Be("1");
        }
    }
}