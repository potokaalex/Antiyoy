using Client.Hex;
using Client.Infrastructure;
using Client.Utilities;
using UnityEngine;

namespace Client.DebugFeatures
{
  public class DebugController : MonoBehaviour, IInitializable
  {
    [SerializeField] private CellDebugController _cellDebugPrefab;
    private Transform _cellsDebugRoot;
    private GridController _gridController;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      CreateCells();
    }

    private void CreateCells()
    {
      _cellsDebugRoot = new GameObject("Debug").transform;
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
      {
        var array2DIndex = new Vector2Int(x, y);
        var hexPosition = HexCoordinates.FromArray2DIndex(array2DIndex);
        var worldPosition = _gridController.HexPositionToWorld(hexPosition);
        var instance = Instantiate(_cellDebugPrefab, worldPosition, Quaternion.identity, _cellsDebugRoot);
        instance.gameObject.name = $"Cell-{MathUtilities.ToArrayIndex(array2DIndex, size.x)}";
        instance.Initialize(hexPosition);
      }
    }
  }
}