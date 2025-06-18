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
        private MapEditorPanel _mapEditorPanel;

        public MapController Map { get; private set; }

        public void Initialize(MapController map, MapEditorPanel mapEditorPanel)
        {
            _mapEditorPanel = mapEditorPanel;
            Map = map;
            Text.SetText(map.Name);
        }

        public void Select() => Image.color = Selected;

        public void UnSelect() => Image.color = Default;

        protected override void OnClick() => _mapEditorPanel.Select(this);
    }
}