using System;
using System.Collections.Generic;
using ClientCode.Gameplay.Region;
using ClientCode.UI.Controllers;

namespace ClientCode.Data.Saved
{
    [Serializable]
    public class RegionSavedData
    {
        public List<int> CellsId;
        public RegionType Type;
    }
}