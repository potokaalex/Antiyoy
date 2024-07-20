using System.Collections.Generic;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Region;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ClientCode.Data.Static.Config
{
    [CreateAssetMenu(menuName = "Configs/Gameplay", fileName = "GameplayConfig", order = 0)]
    public class GameplayConfig : SerializedScriptableObject
    {
        public Dictionary<RegionType, Color> RegionColors;
        public GridObject GridObject;
        public TileBase EmptyTile;
        public TileBase Tile;
        public CellDebugObject CellDebug;
    }
}