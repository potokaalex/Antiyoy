using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Player;
using Client.Project.Region;

namespace Client.Project.Map.Data
{
  [Serializable]
  public struct PlayerData
  {
    public List<RegionData> Regions;
    public PlayerType Type;
    public RegionType RegionType;

    public PlayerData Copy()
    {
      return new PlayerData
      {
        Regions = Regions.Select(r => r.Copy()).ToList(),
        Type = Type,
        RegionType = RegionType
      };
    }
  }
}