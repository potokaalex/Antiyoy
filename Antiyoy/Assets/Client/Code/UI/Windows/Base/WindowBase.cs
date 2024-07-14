using UnityEngine;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowBase : MonoBehaviour, IUIElement
    {
        public bool IsOpen { get; private set; }

        public virtual void Open()
        {
            IsOpen = true;
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            IsOpen = false;
            gameObject.SetActive(false);
        }
    }
}