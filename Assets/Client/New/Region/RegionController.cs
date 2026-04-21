using System.Collections.Generic;

namespace Client.New.Region
{
  public class RegionController
  {
    private readonly List<CellController> _cells = new();
    private readonly RegionsFactory _regionsFactory;

    public IReadOnlyList<CellController> Cells => _cells;

    public RegionType Type { get; set; }

    public RegionController(RegionsFactory regionsFactory)
    {
      _regionsFactory = regionsFactory;
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
  }
}