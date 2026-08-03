using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;

namespace Client.ActionsHistory.Actions
{
  public class MoveUnitAction : IHistoryAction
  {
    private readonly UnitsService _unitsService;
    private readonly RegionsService _regionsService;
    private readonly CellController _newCell;
    private readonly CellController _oldCell;
    private readonly RegionType _oldRegionType;
    private readonly UnitType _unitType;

    public MoveUnitAction(CellController newCell, CellController oldCell, RegionType oldRegionType, UnitType unitType)
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _newCell = newCell;
      _oldCell = oldCell;
      _oldRegionType = oldRegionType;
      _unitType = unitType;
    }

    public void Undo()
    {
      _unitsService.Destroy(_newCell.Unit);
      _regionsService.SetRegionType(_newCell, _oldRegionType);
      _unitsService.Create(_oldCell, _unitType);
    }
  }
}