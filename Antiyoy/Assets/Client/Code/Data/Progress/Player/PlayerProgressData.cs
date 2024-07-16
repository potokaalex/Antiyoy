using System;
using ClientCode.Data.Progress.Player.Map;

namespace ClientCode.Data.Progress.Player
{
    public class PlayerProgressData
    {
        public string SelectedMapKey = string.Empty;
        public string[] MapKeys = Array.Empty<string>();
        public MapProgressData Map = new();
    }
}