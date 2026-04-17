using Client.New.Cell;
using Client.New.Hex;
using Client.New.Region;
using Zenject;

namespace Client.New
{
  public class GameController : IInitializable
  {
    private readonly GridController _gridController;
    private readonly RegionsService _regionsService;

    public GameController(GridController gridController, RegionsService regionsService)
    {
      _gridController = gridController;
      _regionsService = regionsService;
    }

    public void Initialize()
    {
      _gridController.Initialize();
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