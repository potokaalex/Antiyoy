using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Buttons
{
    public class WindowToggle : ButtonBase
    {
        [SerializeField] private WindowType _windowType;
        private WindowsFactory _windowsFactory;

        [Inject]
        public void Construct(WindowsFactory windowsFactory) => _windowsFactory = windowsFactory;

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