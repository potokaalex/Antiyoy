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
    private readonly UnitType _movingUnitType;
    private SetRegionTypeResult _setRegionTypeResult;

    public MoveUnitAction(CellController newCell, UnitType? newCellUnitType, CellController oldCell, UnitType movingUnitType,
      SetRegionTypeResult setRegionTypeResult)
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _newCell = newCell;
      _newCellUnitType = newCellUnitType;
      _oldCell = oldCell;
      _movingUnitType = movingUnitType;
      _setRegionTypeResult = setRegionTypeResult;
    }

    public void Undo()
    {
      _unitsService.Destroy(_newCell.Unit);
      if(_newCellUnitType.HasValue)
        _unitsService.Create(_newCell, _newCellUnitType.Value);

      foreach (var region in _setRegionTypeResult.AffectedRegions)
        _regionsService.RestoreRegion(region);
      _setRegionTypeResult.Dispose();

      _unitsService.Create(_oldCell, _movingUnitType);
    }

    public void Dispose() => _setRegionTypeResult.Dispose();
  }
}