using System;
using Client.Project.Region;

namespace Client.Project
{
  [Serializable]
  public struct PlayerRegionType
  {
    public readonly RegionType RegionType;
    public readonly bool IsRandom;

    public PlayerRegionType(RegionType regionType = RegionType.Neutral, bool isRandom = false)
    {
      RegionType = regionType;
      IsRandom = isRandom;
    }
  }
}