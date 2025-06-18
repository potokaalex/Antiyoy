using System;
using System.Collections.Generic;
using Client.Code.Services;
using Client.Code.Services.UI.Window;
using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using ClientCode.UI.Buttons.Map;
using UniRx;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Writing
{
    public class MapEditorPanel : WindowView, IDisposable//TODO: select class.
    {
        public ButtonView CreateButton;
        public ButtonView RemoveButton;
        public ButtonView BackButton;
        public Transform SelectButtonsRoot;
        public MapEditorSelectButton SelectButtonPrefab;
        private readonly List<MapEditorSelectButton> _selectButtons = new();
        private readonly CompositeDisposable _disposables = new();
        private MapsContainer _mapsContainer;
        private Instantiator _instantiator;
        private MapsFactory _mapsFactory;
        private MapEditorSelectButton _selectedButton;

        [Inject]
        public void Construct(MapsContainer mapsContainer, Instantiator instantiator, MapsFactory mapsFactory)
        {
            _mapsFactory = mapsFactory;
            _instantiator = instantiator;
            _mapsContainer = mapsContainer;
        }

        public void Initialize()
        {
            Close();
            BackButton.OnClickEvent.Subscribe(_ => Close()).AddTo(_disposables);
            CreateButton.OnClickEvent.Subscribe(_ => _mapsFactory.Create());
            RemoveButton.OnClickEvent.Subscribe(_ => OnRemove());
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

        protected override void OnClose(bool force) => UnSelect();

        private void UnSelect()
        {
            if (_selectedButton)
            {
                _selectedButton.UnSelect();
                _selectedButton = null;
            }
        }

        private void OnRemove()
        {
            if (_selectedButton)
                _mapsFactory.Destroy(_selectedButton.Map);
        }

        private void ReCreateSelectButtons()//TODO: pool
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