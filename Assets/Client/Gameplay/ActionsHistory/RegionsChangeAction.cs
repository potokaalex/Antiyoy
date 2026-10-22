using Client.Gameplay.Recovery;
using Client.Project.Infrastructure;

namespace Client.Gameplay.ActionsHistory
{
  public class RegionsChangeAction : IHistoryAction
  {
    private readonly RecoveryController _recoveryController;
    private readonly RegionsRecoveryData _recoveryData;

    public RegionsChangeAction(RegionsRecoveryData recoveryData)
    {
      _recoveryData = recoveryData;
      _recoveryController = Locator.Get<RecoveryController>();
    }

    public void Undo() => _recoveryController.RestoreRegions(_recoveryData);

    public void Dispose() => _recoveryController.Dispose(_recoveryData);
  }
}