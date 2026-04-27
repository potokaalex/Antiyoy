using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit
{
  public class UnitController : MonoBehaviour
  {
    private GridController _gridController;
    private CellController _cell;
    private UnitsService _unitsService;

    public CellController Cell => _cell;

    public void Initialize(CellController cell)
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _cell = cell;
    }

    public void GetPositionsInMoveRadius(List<HexCoordinates> outPositions)
    {
      outPositions.Clear();
      using (ListPool<CellController>.Get(out var cells))
      {
        _gridController.GetCellsInRadius(_cell, 1, cells);
        foreach (var cell in cells) 
          outPositions.Add(cell.Position);
      }
    }

    public void Move(CellController cell)
    {
      var friendlyRegion = _cell.Region.Type == cell.Region.Type;

      if (friendlyRegion && cell.Unit)
        return;

      if (!friendlyRegion)
      {
        _unitsService.TryDestroy(cell.Unit);
        cell.ChangeRegionType(_cell.Region.Type);
      }

      _cell.Unit = null;
      _cell = cell;
      _cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(_cell.Position);
    }
  }
}