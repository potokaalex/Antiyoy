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

        public WindowBase Get(WindowType type, bool isNew)
        {
            if (!_windows.TryGetValue(type, out var windows) || windows.Count == 0 || isNew)
                return Create(type);

            return windows[0];
        }

        public virtual void OnBeforeOpen(WindowBase window)
        {
        }

        private WindowBase Create(WindowType type)
        {
            var window = _factory.CreateWindow(type, _windowsRoot);

            if (!_windows.TryGetValue(type, out var windows))
                _windows.Add(type, new List<WindowBase> { window });
            else
                windows.Add(window);

            return window;
        }
    }
}