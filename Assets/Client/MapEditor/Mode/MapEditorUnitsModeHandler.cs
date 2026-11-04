using Client.Project;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Unit.Code;
using Client.Project.Unit.Code.Capital;

namespace Client.MapEditor.Mode
{
  public class MapEditorUnitsModeHandler : IMapEditorModeHandler
  {
    private readonly InputController _inputController;
    private readonly GridController _gridController;
    private readonly UnitsService _unitsService;
    private readonly CapitalsController _capitalsController;
    private UnitType _unitType;
    private bool _destroy;

    public MapEditorUnitsModeHandler()
    {
      _inputController = Locator.Get<InputController>();
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _capitalsController = Locator.Get<CapitalsController>();
    }

    public void Setup(UnitType unitType, bool destroy = false)
    {
      _unitType = unitType;
      _destroy = destroy;
    }

    public void HandleInput(HexCoordinates position)
    {
      if (_inputController.IsClick)
      {
        if (_gridController.GetCell(position, out var cell))
        {
          if (_destroy)
            _unitsService.Destroy(cell.Unit);
          else
            _unitsService.Create(cell, CalculateType(cell, _unitType));
        }
      }
    }

    private UnitType CalculateType(CellController cell, UnitType newUnit)
    {
      if (!_capitalsController.HasCapital(cell.Region) && newUnit == UnitType.Farm)
        return UnitType.Capital;

      if (!cell.HasUnit)
        return newUnit;

      var current = cell.Unit.Type;

      if (current == UnitType.Capital && newUnit == UnitType.Farm)
        return UnitType.Capital;

      if (current == UnitType.Tower && newUnit == UnitType.Tower)
        return UnitType.StrongTower;

      if (current == UnitType.StrongTower && newUnit == UnitType.Tower)
        return UnitType.Tower;

      if (current.IsWarrior() && newUnit.IsWarrior())
      {
        var joinId = _unitsService.GetJoinId(current) + _unitsService.GetJoinId(newUnit);
        var maxJoinId = _unitsService.GetMaxJoinId();
        if (joinId > maxJoinId)
          joinId = joinId % maxJoinId;

        return _unitsService.GetTypeByJoinId(joinId, newUnit);
      }

      return newUnit;
    }
  }
}