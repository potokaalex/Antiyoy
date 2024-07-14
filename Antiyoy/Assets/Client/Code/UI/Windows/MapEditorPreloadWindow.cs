using System.Collections.Generic;
using ClientCode.UI.Buttons;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows
{
    public class MapEditorPreloadWindow : WindowBase
    {
        [SerializeField] private Transform _selectMapButtonsRoot;
        private readonly List<SelectMapButton> _selectMapButtons = new();
        private UIFactory _uiFactory;
        private string[] _mapKeys;

        [Inject]
        public void Construct(UIFactory uiFactory) => _uiFactory = uiFactory;

        public void Initialize(string[] mapKeys) => _mapKeys = mapKeys;

        public override void Open()
        {
            CreateButtons();
            base.Open();
        }

        public override void Close()
        {
            RemoveButtons();
            base.Close();
        }

        private void CreateButtons()
        {
            foreach (var mapKey in _mapKeys)
            {
                var newButton = (SelectMapButton)_uiFactory.CreateButton(ButtonType.SelectMapButton, _selectMapButtonsRoot);
                newButton.Initialize(mapKey);
                _selectMapButtons.Add(newButton);
            }
        }
        
        private void RemoveButtons()
        {
            foreach (var button in _selectMapButtons) 
                _uiFactory.Destroy(button);
            _selectMapButtons.Clear();
        }
    }
}