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
    private readonly UnitType? _oldUnitType;
    private readonly int _regionMoney;
    private SetRegionTypeResult _setRegionTypeResult;

    public CreateUnitAction(CellController cell, UnitType? oldUnitType, int regionMoney, SetRegionTypeResult setRegionTypeResult)
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _cell = cell;
      _oldUnitType = oldUnitType;
      _regionMoney = regionMoney;
      _setRegionTypeResult = setRegionTypeResult;
    }

    public void Undo()
    {
      _unitsService.Destroy(_cell.Unit);
      if(_oldUnitType.HasValue)
        _unitsService.Create(_cell, _oldUnitType.Value);

      _cell.Region.Money = _regionMoney;
      foreach (var region in _setRegionTypeResult.AffectedRegions)
        _regionsService.RestoreRegion(region);

      Dispose();
    }

    public void Dispose() => _setRegionTypeResult.Dispose();
  }
}