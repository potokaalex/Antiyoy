using System;
using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.UI.Windows.Base;
using Sirenix.Serialization;

namespace ClientCode.Data.Static
{
    [Serializable]
    public class Prefabs
    {
        public CellObject CellObject;
        [OdinSerialize] public Dictionary<WindowType, WindowBase> Windows;
    }
}