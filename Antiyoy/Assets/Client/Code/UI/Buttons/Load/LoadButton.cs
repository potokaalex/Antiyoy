using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons.Load
{
    public class LoadButton : ButtonBase
    {
        [SerializeField] private LoadButtonType _loadButtonType;
        private ILoadButtonHandler _handler;

        [Inject]
        public void Construct(ILoadButtonHandler handler) => _handler = handler;

        private protected override void OnClick() => _handler?.Handle(_loadButtonType);
    }
}