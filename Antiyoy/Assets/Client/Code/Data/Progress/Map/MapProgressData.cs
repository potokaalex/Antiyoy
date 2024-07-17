using System.Collections.Generic;
using ClientCode.Data.Saved;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Data.Progress.Map
{
    public class MapProgressData : IProgressData
    {
        public string Key = string.Empty;
        public int Width = 10;
        public int Height = 10;
        public List<TileSavedData> Tiles = new();
        public List<RegionSavedData> Regions = new();
    }
}