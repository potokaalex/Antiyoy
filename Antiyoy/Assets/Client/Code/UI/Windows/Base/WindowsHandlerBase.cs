using System.Collections.Generic;
using UnityEngine;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowsHandlerBase : IWindowsHandler
    {
        private readonly UIFactory _factory;
        private readonly Transform _windowsRoot;
        private readonly Dictionary<WindowType, WindowBase> _windows = new();

        private protected WindowsHandlerBase(UIFactory factory, Transform windowsRoot)
        {
            _factory = factory;
            _windowsRoot = windowsRoot;
        }

        public WindowBase Get(WindowType type)
        {
            if (!_windows.ContainsKey(type))
                Create(type);
            return _windows[type];
        }

        public abstract void OnBeforeOpen(WindowBase window);
        
        private void Create(WindowType type)
        {
            var window = _factory.CreateWindow(type, _windowsRoot);
            _windows.Add(type, window);
        }
    }
}