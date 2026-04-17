using System.Collections.Generic;
using Client.New.Cell;

namespace Client.New.Region
{
  public class RegionController
  {
    public List<CellController> Cells { get; } = new();
    public RegionType Type { get; }

    public RegionController(List<CellController> cells, RegionType type = RegionType.Default)
    {
      Type = type;
      foreach (var cell in cells)
      {
        Add(cell);
      }
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
      //check for destroy region.
    }
  }
}