using System.Collections.Generic;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Map;
using ClientCode.UI.Windows.Base;
using UnityEngine;

namespace ClientCode.UI.Windows.Writing
{
    public class MapEditorPreloadWindow : WindowBase
    {
        [SerializeField] private Transform _selectMapButtonsRoot;
        private readonly List<MapSelectButton> _selectMapButtons = new();
        private IButtonsHandler _buttonsHandler;

        public void Initialize(IButtonsHandler buttonsHandler) => _buttonsHandler = buttonsHandler;

        public void CreateButton(string mapKey)
        {
            //TODO: create button manual in editor and init its ?
            //var newButton = (MapSelectButton)_buttonsFactory.Create(ButtonType.MapSelect, _selectMapButtonsRoot, _buttonsHandler);
            //newButton.Initialize(mapKey);
            //_selectMapButtons.Add(newButton);
        }

        public void RemoveButton(string mapKey)
        {
            //_buttonsFactory.Destroy(_selectMapButtons.Find(b => b.MapKey == mapKey));
        }
    }
}