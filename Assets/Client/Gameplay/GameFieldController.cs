using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;

namespace Client.Gameplay
{
  public class GameFieldController : IInitializable
  {
    private readonly List<CellController> _unitCreationAreaBuffer = new();
    private UnitsService _unitsService;
    private RegionsService _regionsService;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
    }

    public bool CanCreateUnit(UnitType unitType, CellController cell, RegionController playerRegion)
    {
      var cost = _unitsService.GetCost(unitType);
      _unitsService.GetUnitCreationArea(playerRegion, _unitCreationAreaBuffer, unitType);
      return playerRegion.Money >= cost && _unitCreationAreaBuffer.Contains(cell) && _unitsService.CanCreate(cell, playerRegion.Type);
    }

    public void CreateUnit(UnitType unitType, CellController cell, RegionController playerRegion, ref SetRegionTypeResult setRegionTypeResult)
    {
      if (CanCreateUnit(unitType, cell, playerRegion))
      {
        var cost = _unitsService.GetCost(unitType);
        var hasTurns = cell.Region.Type == playerRegion.Type;
        _regionsService.SetRegionType(cell, playerRegion.Type, ref setRegionTypeResult);
        _unitsService.Create(cell, unitType, hasTurns);
        playerRegion.Money -= cost;
      }
    }
  }
}