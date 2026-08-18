using System.Collections.Generic;
using UnityEngine.Pool;

namespace Client.Region
{
  public struct SetRegionTypeResult
  {
    public List<RegionRecoveryData> AffectedRegions;

    public static SetRegionTypeResult Create()
    {
      return new SetRegionTypeResult
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