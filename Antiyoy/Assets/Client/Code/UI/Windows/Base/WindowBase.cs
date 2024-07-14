using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowBase : MonoBehaviour, IUIElement
    {
        private IWindowsHandler _windowsHandler;

        [Inject]
        public void BaseConstruct(IWindowsHandler windowsHandler) => _windowsHandler = windowsHandler;

        public bool IsOpen { get; private set; }
        
        public void Open()
        {
            _windowsHandler.OnBeforeOpen(this);
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