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
    private MapController _mapController;
    
    public void Initialize(MapController mapController)
    {
      _mapController = mapController;
      FillByTile(_tile);
      CreateCells();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      _tilemap.SetColor(GridIndexFrom2DIndex(position.ToArray2DIndex()), color);
    }

    public HexCoordinates WorldPositionToHex(Vector3 worldPosition)
    {
      return HexCoordinates.FromArray2DIndex(GridIndexTo2DIndex(_grid.WorldToCell(worldPosition)));
    }

    public Vector3 HexPositionToWorld(HexCoordinates position)
    {
      return _grid.GetCellCenterWorld(GridIndexFrom2DIndex(position.ToArray2DIndex()));
    }

    public bool TryGetCell(HexCoordinates position, out CellController cell)
    {
      if (_mapController.IsPositionOnMap(position))
      {
        cell = GetCell(position);
        return true;
      }

      cell = null;
      return false;
    }

    private void FillByTile(TileBase tile)
    {
      var mapSize = _mapController.Size;
      for (var y = 0; y < mapSize.y; y++)
      for (var x = 0; x < mapSize.x; x++)
        _tilemap.SetTile(GridIndexFrom2DIndex(new Vector2Int(x, y)), tile);

      _tilemap.CompressBounds();
    }

    private void CreateCells()
    {
      var cellsRoot = new GameObject("CellsRoot").transform;
      var mapSize = _mapController.Size;
      Cells = new CellController[mapSize.x * mapSize.y];

      for (var y = 0; y < mapSize.y; y++)
      for (var x = 0; x < mapSize.x; x++)
      {
        var array2DIndex = new Vector2Int(x, y);
        var position = HexCoordinates.FromArray2DIndex(array2DIndex);
        var worldPosition = HexPositionToWorld(position);
        var cell = Instantiate(_cellPrefab, worldPosition, Quaternion.identity, cellsRoot);
        cell.Initialize(this, position);
        var arrayIndex = MathUtilities.ToArrayIndex(array2DIndex, mapSize.x);
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
      return Cells[MathUtilities.ToArrayIndex(position.ToArray2DIndex(), _mapController.Size.x)];
    }

    private Vector3Int GridIndexFrom2DIndex(Vector2Int index)
    {
      return new Vector3Int(index.y, index.x, 0);
    }

    private Vector2Int GridIndexTo2DIndex(Vector3Int index)
    {
      return new Vector2Int(index.y, index.x);
    }
  }
}