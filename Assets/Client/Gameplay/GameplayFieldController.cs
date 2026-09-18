using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;
using Client.Utilities;

namespace Client.Gameplay
{
  public class GameplayFieldController : IInitializable
  {
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
      _unitsService.GetUnitCreationArea(playerRegion, GameConstants.AreaBuffer, unitType);
      return playerRegion.Money >= cost && GameConstants.AreaBuffer.Contains(cell) && _unitsService.CanCreate(cell, playerRegion.Type, unitType);
    }

    public void CreateUnit(UnitType unitType, CellController cell, RegionController playerRegion)
    {
      if (CanCreateUnit(unitType, cell, playerRegion))
      {
        var cost = _unitsService.GetCost(unitType);
        _unitsService.Create(cell, _unitsService.CalculateJoinedType(unitType, cell, playerRegion.Type), WillHaveTurns(playerRegion.Type, cell));
        _regionsService.SetRegionType(cell, playerRegion.Type);
        playerRegion.Money -= cost;
      }
    }

    public bool CanMoveUnit(IUnit unit, CellController cell)
    {
      unit.GetMoveArea(GameConstants.AreaBuffer);
      return GameConstants.AreaBuffer.Contains(cell) && _unitsService.CanMove(unit, cell);
    }

    public void MoveUnit(IUnit unit, CellController cell)
    {
      if (CanMoveUnit(unit, cell))
      {
        _unitsService.Create(cell, _unitsService.CalculateJoinedType(unit.Type, cell, unit.Cell.Region.Type),
          WillHaveTurns(unit.Cell.Region.Type, cell));
        _unitsService.Destroy(unit);
        _regionsService.SetRegionType(cell, unit.Cell.Region.Type);
      }
    }

    private bool WillHaveTurns(RegionType playerRegion, CellController cell)
    {
      if (cell.Region.Type != playerRegion)
        return false;
      if (cell.HasUnit)
        return cell.Unit.HasTurns;
      return true;
    }
  }
}