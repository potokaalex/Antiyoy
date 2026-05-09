using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class CapitalsController : IInitializable
  {
    private UnitsService _unitsService;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
    }

    public void DestroyCapitalBeforeAddCell(RegionController region, CellController cell)
    {
      if (IsCapital(cell.Unit) && HasCapital(region))
        _unitsService.TryDestroy(cell.Unit);
    }

    public void UpdateCapital(RegionController region)
    {
      if (region.Cells.Count <= 1)
        DestroyCapitals(region);
      else if (region.IsAlive && !HasCapital(region))
      {
        using (ListPool<CellController>.Get(out var cells))
        {
          cells.AddRange(region.Cells);
          cells.SortByIncreasing(x => !x.Unit ? 0 : x.Unit.CapitalReplacementFactor);
          CreateCapital(region, cells[0]);
        }
      }
    }

    public void SetCapital(CellController cell)
    {
      DestroyCapitals(cell.Region);
      CreateCapital(cell.Region, cell);
    }

    public bool IsCapital(UnitController unit) => unit && unit.Type == UnitType.Capital;

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
          _unitsService.TryDestroy(cell.Unit);
    }

    private void CreateCapital(RegionController region, CellController cell)
    {
      _unitsService.TryDestroy(cell.Unit);
      _unitsService.TryCreate(cell, UnitType.Capital, region.Type, out _);
    }
  }
}