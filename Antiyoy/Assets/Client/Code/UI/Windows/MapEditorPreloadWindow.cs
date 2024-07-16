using System.Collections.Generic;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Buttons.Map;
using ClientCode.UI.Factory;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows
{
    public class MapEditorPreloadWindow : WindowBase
    {
        [SerializeField] private Transform _selectMapButtonsRoot;
        private readonly List<MapSelectButton> _selectMapButtons = new();
        private ButtonsFactory _buttonsFactory;
        private IButtonsHandler _buttonsHandler;

        [Inject]
        public void Construct(ButtonsFactory buttonsFactory) => _buttonsFactory = buttonsFactory;

        public void Initialize(IButtonsHandler buttonsHandler) => _buttonsHandler = buttonsHandler;

        public void CreateButton(string mapKey)
        {
            var newButton = (MapSelectButton)_buttonsFactory.Create(ButtonType.MapSelect, _selectMapButtonsRoot, _buttonsHandler);
            newButton.Initialize(mapKey);
            _selectMapButtons.Add(newButton);
        }

        public void RemoveButton(string mapKey) => _buttonsFactory.Destroy(_selectMapButtons.Find(b => b.MapKey == mapKey));
    }
}