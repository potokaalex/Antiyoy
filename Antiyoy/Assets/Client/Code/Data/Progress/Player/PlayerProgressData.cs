using System;

namespace ClientCode.Data.Progress.Player
{
    public class PlayerProgressData
    {
        public string SelectedMapKey = string.Empty;
        public string[] MapKeys = Array.Empty<string>();
        public MapProgressData Map = new();
    }
}