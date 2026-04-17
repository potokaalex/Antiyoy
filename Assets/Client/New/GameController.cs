using Client.New.Cell;
using Client.New.Hex;
using Client.New.Infrastructure;
using Client.New.Region;
using Client.New.Tile;

namespace Client.New
{
  public class GameController : IInitializable
  {
    private GridController _gridController;
    private RegionsService _regionsService;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _regionsService = Locator.Get<RegionsService>();
      Locator.Get<TilemapController>().FillByBaseTiles();
      _gridController.CreateCells();
      _regionsService.CreateRegions();
    }

    public void CreateCell(HexCoordinates position, RegionType type)
    {
      _gridController.CreateCell(position, type);
      _regionsService.TryJoinRegions(position, type);
    }

    public void DestroyCell(CellController cell)
    {
      _gridController.DestroyCell(cell);
      var region = cell.Region;
      cell.Region.Remove(cell);
      _regionsService.TryDivideRegion(region);
    }
  }
}