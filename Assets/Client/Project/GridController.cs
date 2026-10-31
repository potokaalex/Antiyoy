using System.Collections.Generic;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Tile;
using Client.Project.Utilities;
using UnityEngine;

namespace Client.Project
{
  public class GridController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Grid _grid;
    private CellController[] _cells;
    private RegionsService _regionsService;
    private TilemapController _tilemapController;

    public Vector2Int Size { get; private set; }

    public void Initialize()
    {
      _tilemapController = Locator.Get<TilemapController>();
      _regionsService = Locator.Get<RegionsService>();
    }

    public void InitialCreateCells(MapController map)
    {
      Size = map.Size;
      _cells = new CellController[Size.x * Size.y];

      foreach (var cell in map.GetCells())
        CreateCell(cell.Position);

      _tilemapController.FillByBaseTiles();
    }

    public HexCoordinates WorldPositionToHex(Vector3 worldPosition) =>
      HexCoordinates.FromArray2DIndex(GridIndexTo2DIndex(_grid.WorldToCell(worldPosition)));

    public Vector3 HexPositionToWorld(HexCoordinates position) => _grid.GetCellCenterWorld(GridIndexFrom2DIndex(position.ToArray2DIndex()));

    public Vector3Int GridIndexFrom2DIndex(Vector2Int index) => new(index.y, index.x, 0);

    public Vector2Int GridIndexTo2DIndex(Vector3Int index) => new(index.y, index.x);

    public CellController GetCell(HexCoordinates position)
    {
      if (IsPositionInGrid(position))
        return _cells[GetCellIndex(position)];
      return null;
    }

    public bool GetCell(HexCoordinates position, out CellController cell)
    {
      cell = GetCell(position);
      return cell != null;
    }

    public IEnumerable<CellController> GetAllCells()
    {
      foreach (var cell in _cells)
        if (cell != null)
          yield return cell;
    }

    public IEnumerable<CellController> GetNeighbourCells(HexCoordinates aroundPosition)
    {
      foreach (var direction in HexUtilities.Directions)
        if (GetCell(aroundPosition + direction, out var cell))
          yield return cell;
    }

    public void ReCreateCell(HexCoordinates position, RegionType type)
    {
      if (GetCell(position, out var cell))
        _regionsService.SetRegionType(cell, type);
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