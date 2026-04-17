using Client.New.Hex;
using Client.New.Region;
using UnityEngine;
using Zenject;

namespace Client.New
{
  public class MapController : IInitializable
  {
    public Vector2Int Size { get; } = new(10, 10);

    private readonly GridController _gridController;
    private readonly RegionsService _regionsService;

    public MapController(GridController gridController, RegionsService regionsService)
    {
      _gridController = gridController;
      _regionsService = regionsService;
    }

    public void Initialize()
    {
      _gridController.Initialize(this);
      _regionsService.CreateRegions();
    }

    public bool IsPositionOnMap(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return array2DIndex.x >= 0 && array2DIndex.y >= 0 && array2DIndex.x < Size.x && array2DIndex.y < Size.y;
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