using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using ClientCode.UI.Windows.Writing;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace ClientCode.UI.Buttons.Map
{
    public class MapEditorSelectButton : ButtonView
    {
        public TextMeshProUGUI Text;
        public Image Image;
        public Color Default;
        public Color Selected;
        private MapEditorPanelSelector _selector;

        public MapController Map { get; private set; }

        public void Initialize(MapController map, MapEditorPanelSelector mapEditorPanel)
        {
            _selector = mapEditorPanel;
            Map = map;
            Text.SetText(map.Name);
        }

        public void Select() => Image.color = Selected;

        public void UnSelect() => Image.color = Default;

        protected override void OnClick() => _selector.Select(this);
    }
}