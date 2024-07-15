using System;
using ClientCode.Services.SaveLoader.Base;

namespace ClientCode.Services.SaveLoader.Progress
{
    [Serializable]
    public class MapSavedData : ISavedData
    {
        public int Width = 10;
        public int Height = 10;
    }
}