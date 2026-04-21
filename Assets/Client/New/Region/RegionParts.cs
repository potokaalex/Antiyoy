using System.Collections.Generic;
using UnityEngine.Pool;

namespace Client.New.Region
{
  public class RegionParts
  {
    public List<List<CellController>> Items { get; } = new();

    public void Clear()
    {
      foreach (var part in Items)
        ListPool<CellController>.Release(part);
      Items.Clear();
    }

    public void NewPartFrom(List<CellController> cells)
    {
      ListPool<CellController>.Get(out var list);
      list.AddRange(cells);
      Items.Add(list);
    }
  }
}