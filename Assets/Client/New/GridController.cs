using System.Collections.Generic;
using Client.New.Hex;
using Client.New.Region;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Client.New
{
  public class GridController : MonoBehaviour, IGridController
  {
    public CellController[] Cells { get; private set; }

    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    [SerializeField] private CellController _cellPrefab;
    private MapController _mapController;
    private Transform _cellsRoot;
    private TilemapController _tilemapController;

    [Inject]
    public void Construct(TilemapController tilemapController)
    {
      _tilemapController = tilemapController;
    }

    public void Initialize(MapController mapController)
    {
      _mapController = mapController;
      _tilemapController.Initialize(_tilemap, this, mapController);
      _tilemapController.FillByTile(_tile);
      CreateCells();
    }

    public HexCoordinates WorldPositionToHex(Vector3 worldPosition)
    {
      return HexCoordinates.FromArray2DIndex(GridIndexTo2DIndex(_grid.WorldToCell(worldPosition)));
    }

    public Vector3 HexPositionToWorld(HexCoordinates position)
    {
      return _grid.GetCellCenterWorld(GridIndexFrom2DIndex(position.ToArray2DIndex()));
    }

    public Vector3Int GridIndexFrom2DIndex(Vector2Int index)
    {
      return new Vector3Int(index.y, index.x, 0);
    }

    public Vector2Int GridIndexTo2DIndex(Vector3Int index)
    {
      return new Vector2Int(index.y, index.x);
    }

    public bool HasCell(HexCoordinates position)
    {
      return TryGetCell(position, out _);
    }

    public CellController GetCell(HexCoordinates position)
    {
      return Cells[GetCellIndex(position)];
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

    public void CreateCell(HexCoordinates position, RegionType type)
    {
      var worldPosition = HexPositionToWorld(position);
      var cell = Instantiate(_cellPrefab, worldPosition, Quaternion.identity, _cellsRoot);
      cell.Initialize(position, type);
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

    public IEnumerable<CellController> GetNeighbourCells(HexCoordinates aroundPosition)
    {
      foreach (var direction in HexUtilities.Directions)
        if (TryGetCell(aroundPosition + direction, out var cell))
          yield return cell;
    }

    private void CreateCells()
    {
      var mapSize = _mapController.Size;
      Cells = new CellController[mapSize.x * mapSize.y];
      _cellsRoot = new GameObject("CellsRoot").transform;

      for (var y = 0; y < mapSize.y; y++)
      for (var x = 0; x < mapSize.x; x++)
        CreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)), RegionType.Default);
    }

    private int GetCellIndex(HexCoordinates position)
    {
      return MathUtilities.ToArrayIndex(position.ToArray2DIndex(), _mapController.Size.x);
    }
  }
}