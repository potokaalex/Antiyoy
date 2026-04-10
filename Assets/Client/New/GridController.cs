using Client.New.Hex;
using Client.New.Region;
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
    private Transform _cellsRoot;

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
        return cell;
      }

      cell = null;
      return false;
    }

    public bool HasCell(HexCoordinates position)
    {
      return TryGetCell(position, out _);
    }

    public void CreateCell(HexCoordinates position, RegionType type)
    {
      var worldPosition = HexPositionToWorld(position);
      var cell = Instantiate(_cellPrefab, worldPosition, Quaternion.identity, _cellsRoot);
      cell.Initialize(this, position, type);
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), _mapController.Size.x);
      cell.gameObject.name = $"Cell-{arrayIndex}";
      Cells[arrayIndex] = cell;
    }

    public void DestroyCell(CellController cell)
    {
      cell.SetColor(Color.black);
      Destroy(cell.gameObject);
      Cells[GetCellIndex(cell.Position)] = null;
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
      var mapSize = _mapController.Size;
      Cells = new CellController[mapSize.x * mapSize.y];
      _cellsRoot = new GameObject("CellsRoot").transform;

      for (var y = 0; y < mapSize.y; y++)
      for (var x = 0; x < mapSize.x; x++)
        CreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)), RegionType.Default);

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
      return Cells[GetCellIndex(position)];
    }

    private Vector3Int GridIndexFrom2DIndex(Vector2Int index)
    {
      return new Vector3Int(index.y, index.x, 0);
    }

    private Vector2Int GridIndexTo2DIndex(Vector3Int index)
    {
      return new Vector2Int(index.y, index.x);
    }

    private int GetCellIndex(HexCoordinates position)
    {
      return MathUtilities.ToArrayIndex(position.ToArray2DIndex(), _mapController.Size.x);
    }
  }
}