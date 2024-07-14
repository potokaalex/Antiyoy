using ClientCode.UI.Buttons.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons.Map.SaveLoad
{
    public class MapSaveLoadButton : ButtonBase
    {
        [SerializeField] private MapSaveLoadButtonType _type;
        private IMapSaveLoadButtonHandler _handler;

        [Inject]
        public void Construct(IMapSaveLoadButtonHandler handler) => _handler = handler;

        private protected override void OnClick() => _handler.Handle(_type);
    }
}