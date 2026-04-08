using UnityEngine;

namespace Client.New.Gameplay
{
  public class GameplayInstaller : MonoBehaviour
  {
    [SerializeField] private GridController _grid;
    [SerializeField] private CellController _cellPrefab;

    private void Awake()
    {
      var height = 10;
      var width = 10;

      _grid.Initialize(new Vector2Int(width, height));
      var cellsRoot = new GameObject("CellsRoot").transform;

      for (var y = 0; y < height; y++)
      for (var x = 0; x < width; x++)
      {
        var position = HexCoordinates.FromArrayIndex(new Vector2Int(x, y));
        var worldPosition = _grid.GetCellCenterWorld(position);
        var cell = Instantiate(_cellPrefab, worldPosition, Quaternion.identity, cellsRoot);
        cell.Initialize(_grid, position);
      }
    }
  }
}