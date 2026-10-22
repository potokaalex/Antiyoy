using System.Collections.Generic;
using Client.Project.Region;

namespace Client.Gameplay.Recovery
{
  public struct RegionRecoveryData
  {
    public List<CellRecoveryData> Cells;
    public RegionType Type;
    public int Money;
  }
}