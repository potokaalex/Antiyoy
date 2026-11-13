using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Tile;
using Client.Project.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Project
{
  public class GridController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform _background;
    private CellController[] _cells;
    private TilemapController _tilemapController;
    private ObjectPool<CellController> _pool;
    private int _sizeX;
    private int _sizeY;

    public Vector2Int Size => new(_sizeX, _sizeY);

    public LevelSize LevelSize { get; private set; }

    public void Initialize()
    {
      _tilemapController = Locator.Get<TilemapController>();
      _pool = new ObjectPool<CellController>(() => new CellController());
    }

    public void InitialCreateCells(MapController map)
    {
      _sizeX = map.Size.x;
      _sizeY = map.Size.y;

      LevelSize = GameUtilities.FieldSizeToLevelSize(Size);
      _cells = new CellController[_sizeX * _sizeY];
      
      foreach (var cell in map.GetCells())
        CreateCell(cell.Position);

      AdjustBackground();
    }

    public HexCoordinates WorldPositionToHex(Vector3 worldPosition) =>
      HexCoordinates.FromArray2DIndex(GridIndexTo2DIndex(_grid.WorldToCell(worldPosition)));

    public Vector3 HexPositionToWorld(HexCoordinates position) => _grid.GetCellCenterWorld(GridIndexFrom2DIndex(position.ToArray2DIndex()));

    public Vector3Int GridIndexFrom2DIndex(Vector2Int index) => new(index.y, index.x, 0);

    public Vector2Int GridIndexTo2DIndex(Vector3Int index) => new(index.y, index.x);

    public CellController GetCell(HexCoordinates position)
    {
      position.ToArray2DIndex(out var x, out var y);
      if (IsPositionInGrid(x, y))
        return _cells[MathUtilities.ToArrayIndex(x, y, _sizeX)];
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
      var index = position.ToArray2DIndex();
      return IsPositionInGrid(index.x, index.y);
    }

    public CellController CreateCell(HexCoordinates position)
    {
      var cell = _pool.Get();
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
      _cells[arrayIndex] = cell;
      cell.Initialize(position);
      return cell;
    }

    public void DestroyAllCells()
    {
      foreach (var cell in _cells)
        DestroyCell(cell);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsPositionInGrid(int x, int y) => x >= 0 && y >= 0 && x < _sizeX && y < _sizeY;

    private int GetCellIndex(HexCoordinates position)
    {
      var index = position.ToArray2DIndex();
      return MathUtilities.ToArrayIndex(index.x, index.y, Size.x);
    }

    private void AdjustBackground()
    {
      var min = HexPositionToWorld(HexCoordinates.FromArray2DIndex(0, 0));
      var max = HexPositionToWorld(HexCoordinates.FromArray2DIndex(Size.x - 1, Size.y - 1));
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
        _pool.Release(cell);
        _tilemapController.ClearColor(cell.Position);
      }
    }
  }
}