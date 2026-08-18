using System.Collections.Generic;
using UnityEngine.Pool;

namespace Client.Region
{
  public struct RegionRecoveryData
  {
    public List<CellController> Cells;
    public RegionType Type;
    public int Money;
    public CellController CapitalPosition;

    public static RegionRecoveryData Create(RegionController region)
    {
      var cells = ListPool<CellController>.Get();
      cells.AddRange(region.Cells);
      return new RegionRecoveryData
      {
        Cells = cells,
        Type = region.Type,
        Money = region.Money,
        CapitalPosition = region.Capital?.Cell
      };
    }

    public void Dispose() => ListPool<CellController>.Release(Cells);
  }
}