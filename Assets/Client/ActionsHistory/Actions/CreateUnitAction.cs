using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;

namespace Client.ActionsHistory.Actions
{
  public class CreateUnitAction : IHistoryAction
  {
    private readonly UnitsService _unitsService;
    private readonly RegionsService _regionsService;
    private readonly CellController _cell;
    private readonly RegionType _oldRegionType;
    private readonly int _spentMoney;

    public CreateUnitAction(CellController cell, RegionType oldRegionType, int spentMoney)
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _cell = cell;
      _oldRegionType = oldRegionType;
      _spentMoney = spentMoney;
    }

    public void Undo()
    {
      _cell.Region.Money += _spentMoney;
      _unitsService.Destroy(_cell.Unit);
      _regionsService.SetRegionType(_cell, _oldRegionType);
    }
  }
}