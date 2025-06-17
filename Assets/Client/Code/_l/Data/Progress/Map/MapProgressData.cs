using System.Collections.Generic;
using ClientCode.Data.Saved;
using ClientCode.Services.Progress.Base;
using UnityEngine;

namespace ClientCode.Data.Progress.Map
{
    public class MapProgressData : IProgressData
    {
        public string Key = string.Empty;
        public Vector2Int Size = new();
        public List<TileSavedData> Tiles = new();
        public List<RegionSavedData> Regions = new();
    }
}