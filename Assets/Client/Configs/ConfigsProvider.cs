using System.Collections.Generic;
using Client.Region;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Configs
{
  [CreateAssetMenu(menuName = "Client/Configs/ConfigsProvider", fileName = "ConfigsProvider", order = 0)]
  public class ConfigsProvider : SerializedScriptableObject
  {
    public Dictionary<RegionType, Color> RegionsColors;
  }
}