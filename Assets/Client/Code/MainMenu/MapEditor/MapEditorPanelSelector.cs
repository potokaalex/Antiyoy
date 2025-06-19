using System;
using System.Collections.Generic;
using Client.Code.Services;
using ClientCode.UI.Buttons.Map;
using UniRx;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Writing
{
    public class MapEditorPanelSelector : MonoBehaviour, IDisposable
    {
        public Transform SelectButtonsRoot;
        public MapEditorSelectButton SelectButtonPrefab;
        private readonly List<MapEditorSelectButton> _selectButtons = new();
        private readonly CompositeDisposable _disposables = new();
        private MapEditorSelectButton _selectedButton;
        private MapsContainer _mapsContainer;
        private Instantiator _instantiator;

        public bool HasSelection => _selectedButton;

        public MapController SelectedMap => _selectedButton.Map;

        [Inject]
        public void Construct(MapsContainer mapsContainer, Instantiator instantiator)
        {
            _mapsContainer = mapsContainer;
            _instantiator = instantiator;
        }
        
        public void Initialize()
        {
            _mapsContainer.OnChangedEvent.Subscribe(_ => ReCreateSelectButtons()).AddTo(_disposables);
            ReCreateSelectButtons();
        }
        
        public void Dispose() => _disposables.Dispose();

        public void Select(MapEditorSelectButton button)
        {
            if (_selectedButton == button)
            {
                UnSelect();
                return;
            }

            UnSelect();
            _selectedButton = button;
            _selectedButton.Select();
        }
        
        public void UnSelect()
        {
            if (_selectedButton)
            {
                _selectedButton.UnSelect();
                _selectedButton = null;
            }
        }
        
        private void ReCreateSelectButtons() //TODO: pool ?
        {
            UnSelect();

            for (var i = 0; i < _selectButtons.Count; i++)
                Destroy(_selectButtons[i].gameObject);
            _selectButtons.Clear();

            using var d = UnityEngine.Pool.ListPool<MapController>.Get(out var maps);
            _mapsContainer.Get(maps);

            for (var i = 0; i < maps.Count; i++)
            {
                var button = _instantiator.InstantiateForComponent<MapEditorSelectButton>(SelectButtonPrefab.gameObject, SelectButtonsRoot);
                button.Initialize(maps[i], this);
                _selectButtons.Add(button);
            }
        }
    }
}