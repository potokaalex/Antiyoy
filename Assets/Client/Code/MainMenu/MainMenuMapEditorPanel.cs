using System.Collections.Generic;
using Client.Code.Services.UI.Window;
using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using ClientCode.UI.Buttons.Map;
using UnityEngine;

namespace ClientCode.UI.Windows.Writing
{
    public class MainMenuMapEditorPanel : WindowView
    {
        public ButtonView CreateButton;
        public ButtonView RemoveButton;
        public Transform SelectButtonsRoot;
        public MapSelectButton SelectButtonPrefab;
        private readonly List<MapSelectButton> _selectButtons = new();
        
        public void Initialize() => Close();
    }
}