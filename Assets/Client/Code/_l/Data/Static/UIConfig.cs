using System.Collections.Generic;
using ClientCode.Services.CanvasService;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ClientCode.Data.Static
{
    [CreateAssetMenu(menuName = "Configs/UI", fileName = "UIConfig", order = 0)]
    public class UIConfig : SerializedScriptableObject
    {
        public ProjectCanvasObject ProjectCanvasObject;
        public Dictionary<ButtonType, ButtonBaseOld> Buttons;
        public Dictionary<WindowType, WindowBaseOld> Windows;
    }
}