using System;

namespace ClientCode.Data.Progress.Player
{
    public class PlayerProgressData
    {
        public string[] MapKeys = Array.Empty<string>();
        public string SelectedMapKey;
        public MapProgressData Map = new();
    }
}