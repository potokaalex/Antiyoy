using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowBase : MonoBehaviour, IUIElement
    {
        private IWindowsHandler _windowsHandler;

        [Inject]
        public void BaseConstruct(IWindowsHandler windowsHandler, WindowType type)
        {
            _windowsHandler = windowsHandler;
            Type = type;
        }

        public WindowType Type { get; private set; }

        public bool IsOpen { get; private set; }

        public void Open()
        {
            _windowsHandler.OnBeforeOpen(this);
            OnOpen();
        }

        public void Close()
        {
            OnClose();
            _windowsHandler.OnAfterClose(this);
        }

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