using System.Collections.Generic;
using Client.ActionsHistory.Actions;
using Client.Gameplay;
using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;

namespace Client.ActionsHistory
{
  public class ActionsHistoryController : IInitializable
  {
    private readonly Stack<IHistoryAction> _actions = new();
    private GameplayController _gameplayController;

    public void Initialize() => _gameplayController = Locator.Get<GameplayController>();

    public void Undo()
    {
      if (_actions.TryPop(out var action))
      {
        action.Undo();
        _gameplayController.SelectLastSelectedRegion();
      }
    }

    public void Clear()
    {
      foreach (var action in _actions)
        action.Dispose();

      _actions.Clear();
    }

    public void CreateUnit(CellController cell, UnitType? oldUnitType, int regionMoney, SetRegionTypeResult setRegionTypeResult) =>
      _actions.Push(new CreateUnitAction(cell, oldUnitType, regionMoney, setRegionTypeResult));

    public void MoveUnit(CellController newCell, UnitType? newCellUnitType, CellController oldCell, UnitType movingUnitType,
      SetRegionTypeResult setRegionTypeResult) =>
      _actions.Push(new MoveUnitAction(newCell, newCellUnitType, oldCell, movingUnitType, setRegionTypeResult));
  }
}