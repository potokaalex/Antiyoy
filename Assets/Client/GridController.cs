using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using UnityEngine;

namespace Client
{
  public class GridController : MonoBehaviour
  {
    [SerializeField] private Grid _grid;
    private RegionsService _regionsService;

    public Vector2Int Size { get; } = new(10, 10);

    public CellController[] Cells { get; private set; }

    private void Awake()
    {
      _regionsService = Locator.Get<RegionsService>();
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
        return cell != null;
      }

      cell = null;
      return false;
    }

    public void CreateCells()
    {
      Cells = new CellController[Size.x * Size.y];

      for (var y = 0; y < Size.y; y++)
      for (var x = 0; x < Size.x; x++)
        CreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)), RegionType.Neutral);
    }

    public void CreateCell(HexCoordinates position, RegionType type)
    {
      var cell = new CellController();
      cell.Initialize(position, type);
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
      Cells[arrayIndex] = cell;
      _regionsService.TryJoinRegions(position, type);
    }

    public void DestroyCell(CellController cell)
    {
      Cells[GetCellIndex(cell.Position)] = null;
      _regionsService.RemoveFromRegionAndTryDivideRegion(cell);
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

    private int GetCellIndex(HexCoordinates position)
    {
      return MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
    }
  }
}