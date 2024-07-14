using ClientCode.UI.Buttons.Base;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons.Map.Select
{
    public class MapSelectButton : ButtonBase
    {
        [SerializeField] private TextMeshProUGUI _text;
        private string _mapKey;
        private IMapSelectButtonHandler _handler;

        [Inject]
        public void Construct(IMapSelectButtonHandler handler) => _handler = handler;

        public void Initialize(string mapKey)
        {
            _mapKey = mapKey;
            _text.text = mapKey;
        }

        private protected override void OnClick() => _handler.Handle(_mapKey);
    }
}