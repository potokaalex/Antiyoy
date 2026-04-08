using Client.New.Hex;
using UnityEngine;
using Zenject;

namespace Client.New
{
  public class MapController : IInitializable
  {
    public Vector2Int Size { get; } = new(10, 10);

    private readonly GridController _gridController;

    public MapController(GridController gridController)
    {
      _gridController = gridController;
    }

    public bool IsPositionOnMap(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return array2DIndex.x >= 0 && array2DIndex.y >= 0 && array2DIndex.x < Size.x && array2DIndex.y < Size.y;
    }

    public void Initialize()
    {
      _gridController.Initialize(this);
    }
  }
}