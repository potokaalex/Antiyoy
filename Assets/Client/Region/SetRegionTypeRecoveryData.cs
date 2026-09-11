using System.Collections.Generic;
using UnityEngine.Pool;

namespace Client.Region
{
  public struct SetRegionTypeRecoveryData
  {
    public List<RegionRecoveryData> AffectedRegions;

    public static SetRegionTypeRecoveryData Create()
    {
      return new SetRegionTypeRecoveryData
      {
        AffectedRegions = ListPool<RegionRecoveryData>.Get()
      };
    }

    public void Dispose()
    {
      foreach (var region in AffectedRegions)
        region.Dispose();

      ListPool<RegionRecoveryData>.Release(AffectedRegions);
    }
  }
}