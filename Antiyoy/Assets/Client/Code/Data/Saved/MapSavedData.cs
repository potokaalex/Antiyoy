using System;
using System.Collections.Generic;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Data.Saved
{
    [Serializable]
    public class MapSavedData : ISavedData
    {
        public int Width;
        public int Height;
        public List<TileSavedData> Tiles = new();
        public List<RegionSavedData> Regions = new();
    }
}