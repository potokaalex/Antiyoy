using System.Collections.Generic;
using UnityEngine;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowsHandler : IWindowsHandler
    {
        private readonly UIFactory _factory;
        private readonly Transform _windowsRoot;
        private readonly Dictionary<WindowType, IWindow> _windows = new();

        private protected WindowsHandler(UIFactory factory, Transform windowsRoot)
        {
            _factory = factory;
            _windowsRoot = windowsRoot;
        }

        public void Toggle(WindowType type)
        {
            if (!_windows.ContainsKey(type))
                Create(type);

            var window = _windows[type];

            if (window.IsOpen)
                window.Close();
            else
                Open(window);
        }

        private protected virtual void Open(IWindow window) => window.Open();

        private void Create(WindowType type)
        {
            var window = _factory.CreateWindow(type, _windowsRoot);
            _windows.Add(type, window);
        }
    }
}