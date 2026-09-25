using System.Collections.Generic;
using Client.Region;

namespace Client.Recovery
{
  public struct RegionRecoveryData
  {
    public List<CellRecoveryData> Cells;
    public RegionType Type;
    public int Money;
  }
}