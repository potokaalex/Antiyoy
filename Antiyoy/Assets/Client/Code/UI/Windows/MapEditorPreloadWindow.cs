using System.Collections.Generic;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Map.Select;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows
{
    public class MapEditorPreloadWindow : WindowBase
    {
        [SerializeField] private Transform _selectMapButtonsRoot;
        private readonly List<MapSelectButton> _selectMapButtons = new();
        private UIFactory _uiFactory;

        [Inject]
        public void Construct(UIFactory uiFactory) => _uiFactory = uiFactory;

        public void CreateButton(string mapKey)
        {
            var newButton = (MapSelectButton)_uiFactory.CreateButton(ButtonType.SelectMapButton, _selectMapButtonsRoot);
            newButton.Initialize(mapKey);
            _selectMapButtons.Add(newButton);
        }
        
        public void RemoveButton(string mapKey) => _uiFactory.Destroy(_selectMapButtons.Find(b => b.MapKey == mapKey));
    }
}