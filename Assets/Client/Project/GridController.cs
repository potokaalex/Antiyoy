using System.Collections.Generic;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Tile;
using Client.Project.Utilities;
using UnityEngine;

namespace Client.Project
{
  public class GridController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform _background;
    private CellController[] _cells;
    private TilemapController _tilemapController;

    public Vector2Int Size { get; private set; }

    public LevelSize LevelSize { get; private set; }

    public void Initialize() => _tilemapController = Locator.Get<TilemapController>();

    public void InitialCreateCells(MapController map)
    {
      Size = map.Size;
      LevelSize = GameUtilities.FieldSizeToLevelSize(Size);
      _cells = new CellController[Size.x * Size.y];

      foreach (var cell in map.GetCells())
        CreateCell(cell.Position);

      _tilemapController.FillByBaseTiles();
      AdjustBackground();
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

    public void DestroyCell(HexCoordinates position)
    {
      if (GetCell(position, out var cell))
        DestroyCell(cell);
    }

    public bool IsPositionInGrid(HexCoordinates position)
    {
      var array2DIndex = position.ToArray2DIndex();
      return array2DIndex.x >= 0 && array2DIndex.y >= 0 && array2DIndex.x < Size.x && array2DIndex.y < Size.y;
    }

    public CellController CreateCell(HexCoordinates position, bool createTile = false)
    {
      var cell = new CellController();
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
      _cells[arrayIndex] = cell;
      cell.Initialize(position);
      if (createTile)
        _tilemapController.CreateTile(position);
      return cell;
    }

    public void DestroyAllCells()
    {
      foreach (var cell in _cells)
        DestroyCell(cell);
    }

    private int GetCellIndex(HexCoordinates position) => MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);

    private void AdjustBackground()
    {
      var min = HexPositionToWorld(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0)));
      var max = HexPositionToWorld(HexCoordinates.FromArray2DIndex(new Vector2Int(Size.x - 1, Size.y - 1)));
      var diff = max - min;
      var position = diff / 2f;
      var scale = new Vector3(diff.x + MathUtilities.HexSide * 2, diff.y + MathUtilities.HexHeight * 2, 1);

      if (Size.y % 2 != 0)
      {
        position += new Vector3(0, MathUtilities.HexHeight / 2f, 0);
        scale += new Vector3(0, MathUtilities.HexHeight, 0);
      }

      scale += new Vector3(0.25f, 0.25f, 0);

      _background.position = position;
      _background.localScale = scale;
    }

    private void DestroyCell(CellController cell)
    {
      if (cell != null)
      {
        _cells[GetCellIndex(cell.Position)] = null;
        cell.Dispose();
        _tilemapController.DestroyTile(cell.Position);
      }
    }
  }
}