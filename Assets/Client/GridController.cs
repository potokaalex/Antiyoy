using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine;

namespace Client
{
  public class GridController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Grid _grid;
    private CellController[] _cells;
    private RegionsService _regionsService;

    public Vector2Int Size { get; } = new(9, 9);

    public void Initialize() => _regionsService = Locator.Get<RegionsService>();

    public HexCoordinates WorldPositionToHex(Vector3 worldPosition) =>
      HexCoordinates.FromArray2DIndex(GridIndexTo2DIndex(_grid.WorldToCell(worldPosition)));

    public Vector3 HexPositionToWorld(HexCoordinates position) => _grid.GetCellCenterWorld(GridIndexFrom2DIndex(position.ToArray2DIndex()));

    public Vector3Int GridIndexFrom2DIndex(Vector2Int index) => new(index.y, index.x, 0);

    public Vector2Int GridIndexTo2DIndex(Vector3Int index) => new(index.y, index.x);

    public bool GetCell(HexCoordinates position, out CellController cell)
    {
      if (IsPositionInGrid(position))
      {
        cell = _cells[GetCellIndex(position)];
        return cell != null;
      }

      cell = null;
      return false;
    }

    public void InitialCreateCells()
    {
      _cells = new CellController[Size.x * Size.y];

      for (var y = 0; y < Size.y; y++)
      for (var x = 0; x < Size.x; x++)
        CreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)));
    }

    public void ReCreateCell(HexCoordinates position, RegionType type)
    {
      if (GetCell(position, out var cell))
        cell.ChangeRegionType(type);
      else
        CreateCell(position, type);
    }

    public void DestroyCell(HexCoordinates position)
    {
      if (GetCell(position, out var cell))
      {
        _cells[GetCellIndex(cell.Position)] = null;
        cell.Dispose();
      }
    }

    public IEnumerable<CellController> GetNeighbourCells(HexCoordinates aroundPosition)
    {
      foreach (var direction in HexUtilities.Directions)
        if (GetCell(aroundPosition + direction, out var cell))
          yield return cell;
    }

    private void CreateCell(HexCoordinates position, RegionType type)
    {
      var cell = CreateCell(position);
      _regionsService.AddToBestNeighbourRegion(type, cell);
    }

    private CellController CreateCell(HexCoordinates position)
    {
      var cell = new CellController();
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
      _cells[arrayIndex] = cell;
      cell.Initialize(position);
      return cell;
    }

    private bool IsPositionInGrid(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return array2DIndex.x >= 0 && array2DIndex.y >= 0 && array2DIndex.x < Size.x && array2DIndex.y < Size.y;
    }

    private int GetCellIndex(HexCoordinates position) => MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
  }
}