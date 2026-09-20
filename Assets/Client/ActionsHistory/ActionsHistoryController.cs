using System.Collections.Generic;
using Client.ActionsHistory.Actions;
using Client.Region;
using Client.Unit.Code;

namespace Client.ActionsHistory
{
  public class ActionsHistoryController
  {
    private readonly Stack<IHistoryAction> _actions = new();

    public bool Undo()
    {
      if (_actions.TryPop(out var action))
      {
        action.Undo();
        return true;
      }

      return false;
    }

    public void Clear()
    {
      foreach (var action in _actions)
        action.Dispose();

      _actions.Clear();
    }

    public void CreateUnit(CellController cell, UnitType? cellOldUnitType, bool? cellOldUnitTurns, int regionMoney,
      SetRegionTypeRecoveryData setRegionTypeRecoveryData) =>
      _actions.Push(new CreateUnitAction(cell, cellOldUnitType, cellOldUnitTurns, regionMoney, setRegionTypeRecoveryData));

    public void MoveUnit(CellController newCell, UnitType? newCellUnitType, bool? newCellUnitTurns, CellController oldCell,
      UnitType movingUnitType, SetRegionTypeRecoveryData setRegionTypeRecoveryData, int oldRegionMoney) =>
      _actions.Push(
        new MoveUnitAction(newCell, newCellUnitType, newCellUnitTurns, oldCell, movingUnitType, setRegionTypeRecoveryData, oldRegionMoney));
  }
}