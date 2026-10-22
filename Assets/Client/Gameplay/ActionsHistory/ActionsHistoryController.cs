using System.Collections.Generic;
using Client.Gameplay.Recovery;
using Client.Project;
using Client.Project.Infrastructure;

namespace Client.Gameplay.ActionsHistory
{
  public class ActionsHistoryController : IInitializable
  {
    private readonly Stack<IHistoryAction> _actions = new();
    private RecoveryController _recoveryController;

    public void Initialize() => _recoveryController = Locator.Get<RecoveryController>();

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

    public void RegionsChange(CellController cell) => _actions.Push(new RegionsChangeAction(_recoveryController.RecoveryRegionsAround(cell)));
  }
}