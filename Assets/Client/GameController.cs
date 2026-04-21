using Client.DebugFeatures;
using Client.Hex;
using Client.Infrastructure;
using Client.Tile;
using Client.Unit;
using UnityEngine;

namespace Client
{
  public class GameController : IInitializable
  {
    public void Initialize()
    {
      Locator.Get<TilemapController>().FillByBaseTiles();
      Locator.Get<GridController>().CreateCells();
      Locator.Get<DebugController>().CreateCells();
      Locator.Get<UnitsService>().Create(Locator.Get<GridController>().GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0))), UnitType.Peasant);
    }
  }
}