using Client.New.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.New
{
  public class GridController : MonoBehaviour
  {
    public CellController[] Cells { get; private set; }

    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    [SerializeField] private CellController _cellPrefab;
    private Vector2Int _mapSize;

    public void Initialize(Vector2Int mapSize)
    {
      _mapSize = mapSize;
      FillByTile(_tile);
      CreateCells();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      var array2DIndex = position.ToArray2DIndex();
      _tilemap.SetColor(new Vector3Int(array2DIndex.y, array2DIndex.x, 0), color);
    }

    public HexCoordinates WorldPositionToHex(Vector3 worldPosition)
    {
      var gridIndex = (Vector2Int)_grid.WorldToCell(worldPosition);
      return HexCoordinates.FromArray2DIndex(new Vector2Int(gridIndex.y, gridIndex.x));
    }

    public Vector3 HexPositionToWorld(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return _grid.GetCellCenterWorld(new Vector3Int(array2DIndex.y, array2DIndex.x, 0));
    }

    public bool TryGetCell(HexCoordinates position, out CellController cell)
    {
      if (IsPositionOnMap(position))
      {
        cell = GetCell(position);
        return true;
      }

      cell = null;
      return false;
    }

    private void FillByTile(TileBase tile)
    {
      for (var y = 0; y < _mapSize.y; y++)
      for (var x = 0; x < _mapSize.x; x++)
        _tilemap.SetTile(new Vector3Int(y, x), tile);

      _tilemap.CompressBounds();
    }

    private void CreateCells()
    {
      var cellsRoot = new GameObject("CellsRoot").transform;
      Cells = new CellController[_mapSize.x * _mapSize.y];

      for (var y = 0; y < _mapSize.y; y++)
      for (var x = 0; x < _mapSize.x; x++)
      {
        var array2DIndex = new Vector2Int(x, y);
        var position = HexCoordinates.FromArray2DIndex(array2DIndex);
        var worldPosition = HexPositionToWorld(position);
        var cell = Instantiate(_cellPrefab, worldPosition, Quaternion.identity, cellsRoot);
        cell.Initialize(this, position);
        var arrayIndex = MathUtilities.ToArrayIndex(array2DIndex, _mapSize.x);
        cell.gameObject.name = $"Cell-{arrayIndex}";
        Cells[arrayIndex] = cell;
      }

      ConnectCells();
    }

    private void ConnectCells()
    {
      foreach (var cell in Cells)
      {
        foreach (var direction in HexUtilities.Directions)
        {
          var neighborPosition = cell.Position + direction;

          if (TryGetCell(neighborPosition, out var neighbourCell))
            cell.NeighbourCells.Add(neighbourCell);
        }
      }
    }

    private CellController GetCell(HexCoordinates position)
    {
      return Cells[MathUtilities.ToArrayIndex(position.ToArray2DIndex(), _mapSize.x)];
    }

    private bool IsPositionOnMap(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return array2DIndex.x >= 0 && array2DIndex.y >= 0 && array2DIndex.x < _mapSize.x && array2DIndex.y < _mapSize.y;
    }
  }
}