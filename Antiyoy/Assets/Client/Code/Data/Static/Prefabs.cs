using System;
using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.UI;
using ClientCode.UI.Buttons;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using Sirenix.Serialization;

namespace ClientCode.Data.Static
{
    [Serializable]
    public class Prefabs
    {
        public CellObject CellObject;
        [OdinSerialize] public Dictionary<WindowType, WindowBase> Windows;
        [OdinSerialize] public Dictionary<ButtonType, ButtonBase> Buttons;
    }
}