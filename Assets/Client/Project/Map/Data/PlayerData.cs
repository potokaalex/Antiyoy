using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Project.Map.Data
{
  [Serializable]
  public struct PlayerData
  {
    public List<RegionData> Regions;

    public PlayerData Copy()
    {
      return new PlayerData
      {
        Regions = Regions.Select(r => r.Copy()).ToList()
      };
    }
  }
}