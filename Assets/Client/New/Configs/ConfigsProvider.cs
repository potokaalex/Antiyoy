using System.Collections.Generic;
using Client.New.Region;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.New.Configs
{
  [CreateAssetMenu(menuName = "Client/Configs/ConfigsProvider", fileName = "ConfigsProvider", order = 0)]
  public class ConfigsProvider : SerializedScriptableObject
  {
    public Dictionary<RegionType, Color> RegionsColors;
  }
}