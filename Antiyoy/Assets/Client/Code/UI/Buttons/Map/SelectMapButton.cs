using ClientCode.UI.Buttons.Base;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons.Map
{
    public class SelectMapButton : ButtonBase
    {
        [SerializeField] private TextMeshProUGUI _text;
        private string _mapKey;
        private ISelectMapButtonHandler _selectMapButtonHandler;

        [Inject]
        public void Construct(ISelectMapButtonHandler selectMapButtonHandler) => _selectMapButtonHandler = selectMapButtonHandler;

        public void Initialize(string mapKey)
        {
            _mapKey = mapKey;
            _text.text = mapKey;
        }

        private protected override void OnClick() => _selectMapButtonHandler.Handle(_mapKey);
    }
}