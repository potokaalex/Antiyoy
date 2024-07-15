using System;
using ClientCode.Services.SaveLoad;

namespace ClientCode.Data
{
    [Serializable]
    public class MapSavedData : ISavedData
    {
        public int Width = 10;
        public int Height = 10;
    }
}