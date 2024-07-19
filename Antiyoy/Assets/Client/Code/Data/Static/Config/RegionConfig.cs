using System.Collections.Generic;
using ClientCode.Gameplay.Region;
using ClientCode.UI.Controllers;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ClientCode.Data.Static.Config
{
    [CreateAssetMenu(menuName = "Configs/Region", fileName = "RegionConfig", order = 0)]

    public class RegionConfig : SerializedScriptableObject
    {
        [OdinSerialize] public Dictionary<RegionType, Color> Colors;
    }
}