using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Windows.Base;
using UnityEngine;

namespace ClientCode.UI.Buttons
{
    public class WindowToggle : ButtonBaseOld
    {
        [SerializeField] private WindowType _windowType;
       
        public override ButtonType GetBaseType() => ButtonType.WindowToggle;

        private protected override void OnClick()
        {
            /*
            var window = _windowsFactory.Get(_windowType);

            if (window.IsOpen)
                window.Close();
            else
                window.Open();
            */
        }
    }
}