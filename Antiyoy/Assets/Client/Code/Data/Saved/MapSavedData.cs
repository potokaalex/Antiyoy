using System;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Data.Saved
{
    [Serializable]
    public class MapSavedData : ISavedData
    {
        public int Width = 10;
        public int Height = 10;
    }
}