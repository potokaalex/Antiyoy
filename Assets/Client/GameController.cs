using Client.DebugFeatures;
using Client.Infrastructure;
using Client.Tile;

namespace Client
{
  public class GameController : IInitializable
  {
    public void Initialize()
    {
      Locator.Get<TilemapController>().FillByBaseTiles();
      Locator.Get<GridController>().CreateCells();
      Locator.Get<DebugController>().CreateCells();
    }
  }
}