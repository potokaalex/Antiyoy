using System.Collections.Generic;
using Client.New.Hex;
using Client.New.Region;
using UnityEngine;

namespace Client.New
{
  public interface IGridController
  {
    CellController[] Cells { get; }
    HexCoordinates WorldPositionToHex(Vector3 worldPosition);
    Vector3 HexPositionToWorld(HexCoordinates position);
    public Vector3Int GridIndexFrom2DIndex(Vector2Int index);
    Vector2Int GridIndexTo2DIndex(Vector3Int index);
    bool HasCell(HexCoordinates position);
    CellController GetCell(HexCoordinates position);
    bool TryGetCell(HexCoordinates position, out CellController cell);
    IEnumerable<CellController> GetNeighbourCells(HexCoordinates aroundPosition);
    void CreateCell(HexCoordinates position, RegionType type);
    void DestroyCell(CellController cell);
  }
}