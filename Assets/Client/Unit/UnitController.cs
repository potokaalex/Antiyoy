using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using UnityEngine;

namespace Client.Unit
{
  public class UnitController : MonoBehaviour
  {
    private GridController _gridController;
    private CellController _cell;

    public void Initialize(CellController cell)
    {
      _gridController = Locator.Get<GridController>();
      _cell = cell;
    }

    public void GetMovePositions(List<HexCoordinates> outList)
    {
      outList.Clear();

      foreach (var cell in _gridController.GetNeighbourCells(_cell.Position))
        outList.Add(cell.Position);
    }

    public void Move(HexCoordinates position)
    {
      var prevCell = _cell;
      _cell.Unit = null;
      _cell = _gridController.GetCell(position);
      _cell.Unit = this;
      if (_cell.Region.Type != prevCell.Region.Type)
        _cell.ChangeRegionType(prevCell.Region.Type);

      transform.position = _gridController.HexPositionToWorld(_cell.Position);
    }
  }
}