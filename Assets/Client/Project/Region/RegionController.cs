using System.Collections.Generic;
using System.Linq;
using Client.Project.Infrastructure;
using Client.Project.Unit.Code;

namespace Client.Project.Region
{
  public class RegionController
  {
    private readonly List<CellController> _cells = new();
    private readonly RegionsFactory _regionsFactory;

    public IReadOnlyList<CellController> Cells => _cells;

    public RegionType Type { get; set; }

    public int Money { get; set; }

    public bool IsAlive => _cells.Count >= 2 && Type != RegionType.Neutral;

    public RegionController() => _regionsFactory = Locator.Get<RegionsFactory>();

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

        if (cell.HasUnit)
          result += cell.Unit.Income;
      }

      return result;
    }

    public int CellsIncome() => _cells.Count;

    public int FarmsIncome() => _cells.Sum(x => x.HasUnit && x.Unit.Type == UnitType.Farm ? x.Unit.Income : 0);

    public int WarriorsIncome() => _cells.Sum(x => x.HasUnit && x.Unit.Type.IsWarrior() ? x.Unit.Income : 0);

    public int TowersIncome() => _cells.Sum(x => x.HasUnit && x.Unit.Type.IsTower() ? x.Unit.Income : 0);

    public int TressIncome() => _cells.Sum(x => x.HasUnit && x.Unit.Type.IsTree() ? x.Unit.Income : 0);

    public void Clear()
    {
      Money = 0;
      _cells.Clear();
    }

    public void SetCell(CellController cell, int index) => _cells[index] = cell;
  }
}