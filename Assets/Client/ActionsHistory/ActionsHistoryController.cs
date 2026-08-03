using System.Collections.Generic;
using Client.ActionsHistory.Actions;
using Client.Gameplay;
using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;
using UnityEngine;

namespace Client.ActionsHistory
{
  public class ActionsHistoryController : IInitializable, ITickable
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

    public void Clear() => _actions.Clear();

    public void CreateUnit(CellController cell, RegionType oldRegionType, int spentMoney) =>
      _actions.Push(new CreateUnitAction(cell, oldRegionType, spentMoney));

    public void MoveUnit(CellController newCell, CellController oldCell, RegionType oldRegionType, UnitType unitType) =>
      _actions.Push(new MoveUnitAction(newCell, oldCell, oldRegionType, unitType));

    public void Tick()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        var str = string.Empty;
        foreach (var action in _actions)
          str += $"{action.GetType().Name}\n";

        UnityEngine.Debug.Log(str);
      }
    }
  }
}