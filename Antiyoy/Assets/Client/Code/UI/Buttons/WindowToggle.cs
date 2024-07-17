using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Factory;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons
{
    public class WindowToggle : ButtonBase
    {
        [SerializeField] private WindowType _windowType;
        private IWindowsFactory _windowsFactory;

        [Inject]
        public void Construct(IWindowsFactory windowsFactory) => _windowsFactory = windowsFactory;

        private protected override void OnClick()
        {
            var window = _windowsFactory.Get(_windowType);

            if (window.IsOpen)
                window.Close();
            else
                window.Open();
        }
    }
}