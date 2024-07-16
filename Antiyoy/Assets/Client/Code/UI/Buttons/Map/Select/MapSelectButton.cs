using ClientCode.UI.Buttons.Base;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons.Map.Select
{
    public class MapSelectButton : ButtonBase
    {
        [SerializeField] private TextMeshProUGUI _text;
        private IMapSelectButtonHandler _handler;

        public string MapKey { get; private set; }
        
        [Inject]
        public void Construct(IMapSelectButtonHandler handler) => _handler = handler;
        
        public void Initialize(string mapKey)
        {
            MapKey = mapKey;
            _text.text = mapKey;
        }

        private protected override void OnClick() => _handler.Handle(MapKey);
    }
}