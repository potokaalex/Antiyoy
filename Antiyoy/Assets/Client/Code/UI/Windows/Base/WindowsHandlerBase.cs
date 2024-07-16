using System.Collections.Generic;
using UnityEngine;

namespace ClientCode.UI.Windows.Base
{
    public abstract class WindowsHandlerBase : IWindowsHandler
    {
        private protected Transform WindowsRoot;
        private readonly UIFactory _factory;
        private readonly Dictionary<WindowType, List<WindowBase>> _windows = new();

        private protected WindowsHandlerBase(UIFactory factory, Transform windowsRoot)
        {
            _factory = factory;
            WindowsRoot = windowsRoot;
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

        private protected virtual void OnCreate(WindowBase window)
        {
            if (!_windows.TryGetValue(window.Type, out var windows))
                _windows.Add(window.Type, new List<WindowBase> { window });
            else
                windows.Add(window);
        }

        private WindowBase Create(WindowType type)
        {
            var window = _factory.CreateWindow(type, WindowsRoot);
            OnCreate(window);
            return window;
        }
    }
}