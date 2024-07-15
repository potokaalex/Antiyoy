using System;

namespace ClientCode.Data.Progress
{
    public class PlayerProgressData
    {
        public string[] MapKeys = Array.Empty<string>();
        public MapProgressData Map = new();
    }
}