using System;
using System.Collections.Generic;
using ClientCode.Gameplay.Region;

namespace ClientCode.Data.Saved
{
    [Serializable]
    public class RegionSavedData
    {
        public List<int> CellsId;
        public RegionType Type;
    }
}