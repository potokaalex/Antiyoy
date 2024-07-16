using System;
using System.Collections.Generic;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Windows
{
    public class WindowsFactory : IDisposable
    {
        private readonly Dictionary<WindowType, List<WindowBase>> _windows = new();
        private readonly UIFactory _factory;
        private readonly IWindowsHandler _handler;

        public WindowsFactory(UIFactory factory, IWindowsHandler handler)
        {
            _factory = factory;
            _handler = handler;
        }

        public WindowBase Get(WindowType type, bool isNew = false)
        {
            if (!_windows.TryGetValue(type, out var windows) || windows.Count == 0 || isNew)
                return Create(type);

            return windows[0];
        }

        private WindowBase Create(WindowType type)
        {
            var window = _factory.CreateWindow(type);

            if (!_windows.TryGetValue(type, out var windows))
                _windows.Add(type, new List<WindowBase> { window });
            else
                windows.Add(window);

            _handler.OnCreate(window);

            return window;
        }

        public void Dispose()
        {
            foreach (var windows in _windows.Values)
            foreach (var window in windows)
                _handler.OnDestroy(window);
        }
    }
}