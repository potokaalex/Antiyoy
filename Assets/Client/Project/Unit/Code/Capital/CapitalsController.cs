using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.Utilities;
using UnityEngine.Pool;

namespace Client.Project.Unit.Code.Capital
{
  public class CapitalsController : IInitializable
  {
    private UnitsService _unitsService;
    private RegionsService _regionsService;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
    }

    public void CheckCapitals()
    {
      foreach (var region in _regionsService.Regions)
      {
        var capital = GetCapital(region);
        if (capital != null)
        {
          foreach (var cell in region.Cells)
          {
            var cellUnit = cell.Unit;
            if (IsCapital(cellUnit) && cellUnit != capital)
              _unitsService.Destroy(cellUnit);
          }
        }
        else if (region.IsAlive)
          CreateCapital(region);
      }
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

    public bool HasCapital(RegionController region)
    {
      foreach (var cell in region.Cells)
        if (IsCapital(cell.Unit))
          return true;
      return false;
    }

    public IUnit GetCapital(RegionController region)
    {
      foreach (var cell in region.Cells)
        if (IsCapital(cell.Unit))
          return cell.Unit;

      return null;
    }

    public void RemoveCapital(RegionController region) => _unitsService.Destroy(GetCapital(region));

    private bool IsCapital(IUnit unit) => unit != null && unit.Type == UnitType.Capital;
  }
}