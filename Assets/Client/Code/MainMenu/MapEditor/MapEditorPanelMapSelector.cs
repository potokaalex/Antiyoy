using Client.Code.Core;
using Client.Code.Core.UI.Buttons.Select;
using Client.Code.Gameplay.Map;
using UniRx;
using UnityEngine;
using Zenject;

namespace Client.Code.MainMenu.MapEditor
{
    public class MapEditorPanelMapSelector : SelectionButtonsController<MapController>
    {
        public MapEditorPanelMapSelectionButton ButtonPrefab;
        public Transform ButtonsRoot;
        private readonly CompositeDisposable _disposables = new();
        private MapsContainer _mapsContainer;
        private Instantiator _instantiator;

        [Inject]
        public void Construct(MapsContainer mapsContainer, Instantiator instantiator)
        {
            _mapsContainer = mapsContainer;
            _instantiator = instantiator;
        }

        public override void Initialize()
        {
            _mapsContainer.OnAddMap.Subscribe(_ => ReCreateSelectButtons()).AddTo(_disposables);
            _mapsContainer.OnRemoveMap.Subscribe(_ => ReCreateSelectButtons()).AddTo(_disposables);
            ReCreateSelectButtons();
        }

        public override void Dispose()
        {
            _disposables.Dispose();
            base.Dispose();
        }

        private void ReCreateSelectButtons()
        {
            UnSelect();
            foreach (var (button, _) in Buttons)
                Destroy(button.gameObject);
            Clear();

            using var d = UnityEngine.Pool.ListPool<MapController>.Get(out var maps);
            _mapsContainer.Get(maps);

            for (var i = 0; i < maps.Count; i++)
            {
                var button = _instantiator.InstantiateForComponent<MapEditorPanelMapSelectionButton>(ButtonPrefab.gameObject, ButtonsRoot);
                var map = maps[i];
                button.Text.SetText(map.Name);
                Add(button, map);
            }
        }
    }
}