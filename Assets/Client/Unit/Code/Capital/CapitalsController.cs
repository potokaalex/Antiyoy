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

    public void DestroyCapital(CellController cell)
    {
      if (IsCapital(cell.Unit))
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

    public void SetCapital(CellController cell)
    {
      DestroyCapitals(cell.Region);
      _unitsService.Create(cell, UnitType.Capital);
    }

    public bool IsCapital(IUnit unit) => unit != null && unit.Type == UnitType.Capital;

    public IUnit GetCapital(RegionController region)
    {
      foreach (var cell in region.Cells)
        if (IsCapital(cell.Unit))
          return cell.Unit;
      return null;
    }

    private bool HasCapital(RegionController region)
    {
      foreach (var cell in region.Cells)
        if (IsCapital(cell.Unit))
          return true;
      return false;
    }

    private void DestroyCapitals(RegionController region)
    {
      foreach (var cell in region.Cells)
        if (IsCapital(cell.Unit))
          _unitsService.Destroy(cell.Unit);
    }
  }
}