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
    private readonly UnitType _unitType;
    private SetRegionTypeResult _setRegionTypeResult;

    public MoveUnitAction(CellController newCell, CellController oldCell, UnitType unitType, SetRegionTypeResult setRegionTypeResult)
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _newCell = newCell;
      _oldCell = oldCell;
      _unitType = unitType;
      _setRegionTypeResult = setRegionTypeResult;
    }

    public void Undo()
    {
      _unitsService.Destroy(_newCell.Unit);

      foreach (var region in _setRegionTypeResult.AffectedRegions)
        _regionsService.RestoreRegion(region);
      _setRegionTypeResult.Dispose();

      _unitsService.Create(_oldCell, _unitType);
    }

    public void Dispose() => _setRegionTypeResult.Dispose();
  }
}