using System;
using System.Collections.Generic;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Data.Saved
{
    [Serializable]
    public class MapSavedData : ISavedData
    {
        public int Width = 10;
        public int Height = 10;
        public List<TileSavedData> Tiles = new();
        public List<RegionSavedData> Regions = new();
    }
}