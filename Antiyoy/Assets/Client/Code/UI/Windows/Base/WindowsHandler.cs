using System.Collections.Generic;
using ClientCode.Data.Scene;

namespace ClientCode.UI.Windows.Base
{
    public class WindowsHandler : IWindowsHandler
    {
        private readonly UIFactory _factory;
        private readonly MainMenuSceneData _sceneData;
        private readonly Dictionary<WindowType, IWindow> _windows = new();

        public WindowsHandler(UIFactory factory, MainMenuSceneData sceneData)
        {
            _factory = factory;
            _sceneData = sceneData;
        }

        public void Toggle(WindowType type)
        {
            if(!_windows.ContainsKey(type))
                Create(type);

            var window = _windows[type];
            
            if (window.IsOpen)
                window.Close();
            else
                window.Open();
        }

        private void Create(WindowType type)
        {
            var window = _factory.CreateWindow(type, _sceneData.UIRoot);
            _windows.Add(type, window);
        }
    }
}