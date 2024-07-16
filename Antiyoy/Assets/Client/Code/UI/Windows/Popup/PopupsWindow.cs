using ClientCode.UI.Windows.Base;
using UnityEngine;

namespace ClientCode.UI.Windows.Popup
{
    public class PopupsWindow : WindowBase
    {
        [SerializeField] private Transform _popupsRoot;
        
        public void Add(string message)
        {
            var popup = (PopupWindow)WindowsHandler.Get(WindowType.Popup, true);
            popup.transform.SetParent(_popupsRoot, false);
            popup.Initialize(message);
        }
    }
}