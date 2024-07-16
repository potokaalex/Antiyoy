using System.Collections.Generic;
using ClientCode.Data.Saved;

namespace ClientCode.Data.Progress.Player.Map
{
    public class MapProgressData
    {
        public string Key = string.Empty;
        public int Width = 10;
        public int Height = 10;
        public List<TileSavedData> Tiles = new();
        public List<RegionSavedData> Regions = new();
    }
}