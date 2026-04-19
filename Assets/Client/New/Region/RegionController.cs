using System.Collections.Generic;

namespace Client.New.Region
{
  public class RegionController
  {
    private readonly RegionsFactory _regionsFactory;

    public List<CellController> Cells { get; } = new();

    public RegionType Type { get; }

    public RegionController(RegionsFactory regionsFactory, List<CellController> cells = null, RegionType type = RegionType.Default)
    {
      Type = type;
      _regionsFactory = regionsFactory;
      if (cells != null)
        foreach (var cell in cells)
          Add(cell);
    }

    public void Add(CellController cell)
    {
      cell.Region = this;
      Cells.Add(cell);
    }

    public void Remove(CellController cell)
    {
      cell.Region = null;
      Cells.Remove(cell);
      if (Cells.Count == 0) 
        _regionsFactory.Destroy(this);
    }
  }
}