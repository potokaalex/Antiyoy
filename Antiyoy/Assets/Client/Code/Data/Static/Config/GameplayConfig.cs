using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Region;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ClientCode.Data.Static.Config
{
    [CreateAssetMenu(menuName = "Configs/Gameplay", fileName = "GameplayConfig", order = 0)]
    public class GameplayConfig : SerializedScriptableObject
    {
        public CellObject CellObject;
        public Dictionary<RegionType, Color> RegionColors;
        public Color CellColor;
        public Color TileColor;
    }
}