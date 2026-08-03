using System.Collections.Generic;
using Client.Region;
using Client.Unit.Code;

namespace Client.ActionsHistory
{
  public class ActionsHistoryController
  {
    private readonly Stack<IHistoryAction> _actions = new();

    public void Undo()
    {
      if (_actions.TryPop(out var action)) 
        action.Undo();
    }

    public void Clear()
    {
      _actions.Clear(); 
    }

    public void SetRegionType(CellController cell, RegionType regionType)
    {
      _actions.Push(new SetCellRegionTypeAction(cell, regionType));
    }
    
    public void CreateUnit(CellController cell, UnitType unitType)
    {
    }

    public void DestroyUnit(CellController cell)
    {
    }
  }
}