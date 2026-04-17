using System.Collections.Generic;
using Client.New.Cell;
using Client.New.Hex;
using Client.New.Region;
using Client.New.Tile;
using UnityEngine;
using Zenject;

namespace Client.New
{
  public class GridController : MonoBehaviour
  {
    [SerializeField] private Grid _grid;
    [SerializeField] private CellController _cellPrefab;
    private Transform _cellsRoot;
    private TilemapController _tilemapController;

    public Vector2Int Size { get; } = new(10, 10);
    public CellController[] Cells { get; private set; }

    [Inject]
    public void Construct(TilemapController tilemapController)
    {
      _tilemapController = tilemapController;
    }

    public void Initialize()
    {
      _tilemapController.Initialize(this);
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
      if (IsPositionInGrid(position))
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
      cell.Initialize(_tilemapController, position, type);
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
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

    private bool IsPositionInGrid(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return array2DIndex.x >= 0 && array2DIndex.y >= 0 && array2DIndex.x < Size.x && array2DIndex.y < Size.y;
    }

    private void CreateCells()
    {
      Cells = new CellController[Size.x * Size.y];
      _cellsRoot = new GameObject("CellsRoot").transform;

      for (var y = 0; y < Size.y; y++)
      for (var x = 0; x < Size.x; x++)
        CreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)), RegionType.Default);
    }

    private int GetCellIndex(HexCoordinates position)
    {
      return MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
    }
  }
}