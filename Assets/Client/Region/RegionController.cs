using System.Collections.Generic;
using Client.Infrastructure;
using Client.Unit;
using Client.Unit.Code;

namespace Client.Region
{
  public class RegionController
  {
    private readonly List<CellController> _cells = new();
    private readonly RegionsFactory _regionsFactory;
    private readonly UnitsService _unitsService;

    public IReadOnlyList<CellController> Cells => _cells;
    public RegionType Type { get; set; }
    public int Money { get; set; }
    public bool IsAlive => _cells.Count >= 2 && Type != RegionType.Neutral;

    public RegionController()
    {
      _regionsFactory = Locator.Get<RegionsFactory>();
      _unitsService = Locator.Get<UnitsService>();
    }

    public void Add(CellController cell)
    {
      cell.Region = this;
      _cells.Add(cell);
    }

    public void Remove(CellController cell)
    {
      cell.Region = null;
      _cells.Remove(cell);
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

    public void OnNextTurn()
    {
      if (_cells.Count <= 1)
      {
        Money = 0;
        DestroyAllUnits();
        return;
      }

      Money += GetIncome();
      if (Money < 0)
      {
        Money = 0;
        DestroyAllUnits();
      }
    }

    public bool SpendMoney(int value)
    {
      if (Money >= value)
      {
        Money -= value;
        return true;
      }

      return false;
    }

    private void DestroyAllUnits()
    {
      foreach (var cell in _cells)
        _unitsService.TryDestroy(cell.Unit);
    }
  }
}