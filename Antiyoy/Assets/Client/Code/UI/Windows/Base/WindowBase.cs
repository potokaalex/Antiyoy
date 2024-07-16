using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowBase : MonoBehaviour, IUIElement
    {
        private protected IWindowsHandler WindowsHandler;

        [Inject]
        public void BaseConstruct(IWindowsHandler windowsHandler, WindowType type)
        {
            WindowsHandler = windowsHandler;
            Type = type;
        }

        public WindowType Type { get; private set; }

        public bool IsOpen { get; private set; }

        public void Open()
        {
            WindowsHandler.OnBeforeOpen(this);
            OnOpen();
        }

        public void Close() => OnClose();

        private protected virtual void OnOpen()
        {
            IsOpen = true;
            gameObject.SetActive(true);
        }

        private protected virtual void OnClose()
        {
            IsOpen = false;
            gameObject.SetActive(false);
        }
    }
}