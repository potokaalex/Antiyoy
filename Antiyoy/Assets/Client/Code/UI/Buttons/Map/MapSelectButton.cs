using ClientCode.UI.Buttons.Base;
using TMPro;
using UnityEngine;

namespace ClientCode.UI.Buttons.Map
{
    public class MapSelectButton : ButtonBase
    {
        [SerializeField] private TextMeshProUGUI _text;

        public string MapKey { get; private set; }

        public void Initialize(string mapKey)
        {
            MapKey = mapKey;
            _text.text = mapKey;
        }

        public override ButtonType GetBaseType() => ButtonType.MapSelect;
    }
}