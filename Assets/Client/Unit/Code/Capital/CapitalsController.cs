using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine.Pool;

namespace Client.Unit.Code.Capital
{
  public class CapitalsController : IInitializable
  {
    private UnitsService _unitsService;

    public void Initialize() => _unitsService = Locator.Get<UnitsService>();

    public void Enable() => _unitsService.OnCreate += OnCreateUnit;

    public void Disable() => _unitsService.OnCreate -= OnCreateUnit;

    private void OnCreateUnit(IUnit unit)
    {
      if (unit.Type == UnitType.Capital)
        foreach (var cell in unit.Cell.Region.Cells)
          if (IsCapital(cell.Unit) && cell != unit.Cell)
            _unitsService.Destroy(cell.Unit);
    }

    public void CreateCapital(RegionController region)
    {
      if (region.IsAlive && !HasCapital(region))
      {
        using (ListPool<CellController>.Get(out var cells))
        {
          cells.AddRange(region.Cells);
          cells.SortByIncreasing(x => !x.HasUnit ? 0 : x.Unit.CapitalReplacementFactor);
          _unitsService.Create(cells[0], UnitType.Capital);
        }
      }
    }

    private bool IsCapital(IUnit unit) => unit != null && unit.Type == UnitType.Capital;

    private bool HasCapital(RegionController region)
    {
      foreach (var cell in region.Cells)
        if (IsCapital(cell.Unit))
          return true;
      return false;
    }
  }
}