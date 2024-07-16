using System.Collections.Generic;
using UnityEngine;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowsHandlerBase : IWindowsHandler
    {
        private readonly UIFactory _factory;
        private readonly Transform _windowsRoot;
        private readonly Dictionary<WindowType, List<WindowBase>> _windows = new();

        private protected WindowsHandlerBase(UIFactory factory, Transform windowsRoot)
        {
            _factory = factory;
            _windowsRoot = windowsRoot;
        }

        public WindowBase Get(WindowType type)
        {
            if (!_windows.TryGetValue(type, out var windows) || windows.Count == 0)
                return Create(type);

            var window = windows[^1];
            windows.RemoveAt(windows.Count - 1);
            return window;
        }

        public virtual void OnBeforeOpen(WindowBase window)
        {
        }

        public virtual void OnAfterClose(WindowBase window)
        {
            if (!_windows.TryGetValue(window.Type, out var windows))
                _windows.Add(window.Type, new List<WindowBase> { window });
            else
                windows.Add(window);
        }

        private WindowBase Create(WindowType type) => _factory.CreateWindow(type, _windowsRoot);
    }
}