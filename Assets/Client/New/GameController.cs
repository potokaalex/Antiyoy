using Client.New.Debug;
using Client.New.Infrastructure;
using Client.New.Region;
using Client.New.Tile;

namespace Client.New
{
  public class GameController : IInitializable
  {
    public void Initialize()
    {
      Locator.Get<TilemapController>().FillByBaseTiles();
      Locator.Get<GridController>().CreateCells();
      Locator.Get<RegionsService>().CreateRegions();
      Locator.Get<DebugController>().CreateCells();
    }
  }
}