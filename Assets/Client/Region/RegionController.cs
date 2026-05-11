using System.Collections.Generic;
using Client.Infrastructure;
using Client.Unit.Code;

namespace Client.Region
{
  public class RegionController
  {
    private readonly List<CellController> _cells = new();
    private readonly RegionsFactory _regionsFactory;
    private readonly UnitsService _unitsService;
    private readonly CapitalsController _capitalsController;

    public IReadOnlyList<CellController> Cells => _cells;
    public RegionType Type { get; set; }
    public int Money { get; set; }
    public bool IsAlive => _cells.Count >= 2 && Type != RegionType.Neutral;

    public RegionController()
    {
      _regionsFactory = Locator.Get<RegionsFactory>();
      _unitsService = Locator.Get<UnitsService>();
      _capitalsController = Locator.Get<CapitalsController>();
    }

    public void Add(CellController cell)
    {
      _capitalsController.DestroyCellCapitalIfRegionHasCapital(this, cell);
      cell.Region = this;
      _cells.Add(cell);
      UpdateBuildings();
    }

    public void Remove(CellController cell)
    {
      cell.Region = null;
      _cells.Remove(cell);
      UpdateBuildings();

      if (Cells.Count == 0)
        _regionsFactory.Destroy(this);
    }

    public int GetIncome()
    {
      var result = 0;
      foreach (var cell in _cells)
      {
        result++;

        if (cell.Unit)
          result += cell.Unit.Income;
      }

      return result;
    }

    public void Update()
    {
      if (_cells.Count <= 1)
      {
        Money = 0;
        DestroyAllUnits();
      }

      Money += GetIncome();
      if (Money < 0)
      {
        Money = 0;
        DestroyAllUnits();
      }

      foreach (var cell in _cells)
        if (cell.Unit)
          cell.Unit.RestTurnsCount();
    }

    private void DestroyAllUnits()
    {
      foreach (var cell in _cells)
        if (!_capitalsController.IsCapital(cell.Unit))
          _unitsService.TryDestroy(cell.Unit);
    }

    private void UpdateBuildings()
    {
      if (_cells.Count <= 1)
        DestroyBuildings();
      else
        _capitalsController.CreateCapital(this);
    }

    private void DestroyBuildings()
    {
      foreach (var c in _cells)
        if (c.Unit && c.Unit.Type is UnitType.Capital or UnitType.Farm)
          _unitsService.TryDestroy(c.Unit);
    }
  }
}