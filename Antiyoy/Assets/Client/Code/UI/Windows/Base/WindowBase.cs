using UnityEngine;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowBase : MonoBehaviour, IWindow
    {
        public bool IsOpen { get; private protected set; }

        public abstract void Close();

        public abstract void Open();
    }
}