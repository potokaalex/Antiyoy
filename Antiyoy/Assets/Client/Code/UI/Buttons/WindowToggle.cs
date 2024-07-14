using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons
{
    public class WindowToggle : ButtonBase
    {
        [SerializeField] private WindowType _windowType;
        private IWindowsHandler _handler;

        [Inject]
        public void Construct(IWindowsHandler handler) => _handler = handler;

        private protected override void OnClick() => _handler.Toggle(_windowType);
    }
}