using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;

namespace Client.ActionsHistory.Actions
{
  public class MoveUnitAction : IHistoryAction
  {
    private readonly UnitsService _unitsService;
    private readonly RegionsService _regionsService;
    private readonly CellController _oldCell;
    private readonly CellController _newCell;
    private readonly UnitType? _newCellUnitType;
    private readonly bool? _newCellUnitHasTurns;
    private readonly UnitType _movingUnitType;
    private readonly int _oldRegionMoney;
    private SetRegionTypeRecoveryData _setRegionTypeRecoveryData;

    public MoveUnitAction(CellController newCell, UnitType? newCellUnitType, bool? newCellUnitHasTurns, CellController oldCell,
      UnitType movingUnitType, SetRegionTypeRecoveryData setRegionTypeRecoveryData, int oldRegionMoney)
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _newCell = newCell;
      _newCellUnitType = newCellUnitType;
      _newCellUnitHasTurns = newCellUnitHasTurns;
      _oldCell = oldCell;
      _movingUnitType = movingUnitType;
      _setRegionTypeRecoveryData = setRegionTypeRecoveryData;
      _oldRegionMoney = oldRegionMoney;
    }

    public void Undo()
    {
      _unitsService.Destroy(_newCell.Unit);
      if(_newCellUnitType.HasValue && _newCellUnitHasTurns.HasValue)
        _unitsService.Create(_newCell, _newCellUnitType.Value, _newCellUnitHasTurns.Value);

      foreach (var region in _setRegionTypeRecoveryData.AffectedRegions)
        _regionsService.RestoreRegion(region);
      _setRegionTypeRecoveryData.Dispose();

      _unitsService.Create(_oldCell, _movingUnitType);
      _oldCell.Region.Money = _oldRegionMoney;
    }

    public void Dispose() => _setRegionTypeRecoveryData.Dispose();
  }
}