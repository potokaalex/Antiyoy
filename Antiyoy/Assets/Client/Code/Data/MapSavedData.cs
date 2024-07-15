using System;
using ClientCode.Services.SaveLoader.Base;

namespace ClientCode.Data
{
    [Serializable]
    public class MapSavedData : ISavedData
    {
        public int Width = 10;
        public int Height = 10;
    }
}