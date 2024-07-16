using ClientCode.UI.Factory;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.UI.Windows.Popup
{
    public class PopupsWindow : WindowBase
    {
        [SerializeField] private Transform _popupsRoot;
        private WindowsFactory _factory;

        [Inject]
        public void Construct(WindowsFactory factory) => _factory = factory;

        public void Add(string message)
        {
            var popup = (PopupWindow)_factory.Get(WindowType.Popup, true);
            popup.transform.SetParent(_popupsRoot, false);
            popup.Initialize(message);
        }
    }
}