using System.Collections.Generic;
using Client.Infrastructure;
using Client.Unit.Code;
using UnityEngine.Pool;
using Client.Utilities;

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
      CreateCapital();

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
        DestroyAllUnits(true);
        return;
      }

      Money += GetIncome();
      if (Money < 0)
      {
        Money = 0;
        DestroyAllUnits();
      }
    }

    public void CreateCapital()
    {
      if (IsAlive && !HasCapital())
      {
        using (ListPool<CellController>.Get(out var cells))
        {
          cells.AddRange(_cells);
          cells.SortByIncreasing(x => !x.Unit ? 0 : x.Unit.CapitalReplacementFactor);
          var cell = cells[0];
          _unitsService.TryDestroy(cell.Unit);
          _unitsService.TryCreate(cell, UnitType.Capital, Type, out _);
        }
      }
    }

    public void DestroyCapital()
    {
      if (IsAlive)
      {
        foreach (var cell in _cells)
          if (cell.Unit && cell.Unit.Type == UnitType.Capital)
            _unitsService.TryDestroy(cell.Unit);
      }
    }

    private void DestroyAllUnits(bool withCapital = false)
    {
      foreach (var cell in _cells)
      {
        var isCapital = cell.Unit && cell.Unit.Type == UnitType.Capital;
        if (!isCapital || !withCapital)
          _unitsService.TryDestroy(cell.Unit);
      }
    }

    private bool HasCapital()
    {
      foreach (var cell in _cells)
        if (cell.Unit && cell.Unit.Type == UnitType.Capital)
          return true;
      return false;
    }
  }
}