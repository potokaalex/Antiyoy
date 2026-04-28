using System.Collections.Generic;
using Client.Hex;
using Client.Region;
using Client.Utilities;
using UnityEngine;

namespace Client
{
  public class GridController : MonoBehaviour
  {
    [SerializeField] private Grid _grid;
    private CellController[] _cells;

    public Vector2Int Size { get; } = new(9, 9);

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

    public bool TryGetCell(HexCoordinates position, out CellController cell)
    {
      if (IsPositionInGrid(position))
      {
        cell = _cells[GetCellIndex(position)];
        return cell != null;
      }

      cell = null;
      return false;
    }

    public void CreateCells()
    {
      _cells = new CellController[Size.x * Size.y];

      for (var y = 0; y < Size.y; y++)
      for (var x = 0; x < Size.x; x++)
        CreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)), RegionType.Neutral);
    }

    public void ReCreateCell(HexCoordinates position, RegionType type)
    {
      if (TryGetCell(position, out var cell))
        cell.ChangeRegionType(type);
      else
        CreateCell(position, type);
    }

    public void TryDestroyCell(HexCoordinates position)
    {
      if (TryGetCell(position, out var cell))
      {
        _cells[GetCellIndex(cell.Position)] = null;
        cell.Dispose();
      }
    }

    public IEnumerable<CellController> GetNeighbourCells(HexCoordinates aroundPosition)
    {
      foreach (var direction in HexUtilities.Directions)
        if (TryGetCell(aroundPosition + direction, out var cell))
          yield return cell;
    }

    public void GetCellsInRadius(CellController center, int radius, List<CellController> outCells)
    {
      outCells.Clear();

      using (StackPool<CellController>.Get(out var front))
      {
        front.Push(center);
        outCells.Add(center);
        while (front.Count > 0)
        {
          var cell = front.Pop();
          foreach (var neighbour in GetNeighbourCells(cell.Position))
          {
            if ((neighbour.Position - center.Position).GetMagnitude() <= radius && !outCells.Contains(neighbour))
            {
              outCells.Add(neighbour);
              front.Push(neighbour);
            }
          }
        }
      }
    }

    private void CreateCell(HexCoordinates position, RegionType type)
    {
      var cell = new CellController();
      var arrayIndex = MathUtilities.ToArrayIndex(position.ToArray2DIndex(), Size.x);
      _cells[arrayIndex] = cell;
      cell.Initialize(position, type);
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