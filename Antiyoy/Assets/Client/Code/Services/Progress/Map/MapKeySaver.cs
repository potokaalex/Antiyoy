using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Services.Progress.Actors;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.Services.Progress.Map
{
    public class MapKeySaver : IProgressWriter
    {
        private readonly IWindowsHandler _windowsHandler;

        public MapKeySaver(IWindowsHandler windowsHandler) => _windowsHandler = windowsHandler;

        public async Task OnSave(PlayerProgressData progress)
        {
            var mapKey = progress.Map.Key;

            if (string.IsNullOrEmpty(mapKey)) 
                mapKey = await GetNewKey();

            progress.Map.Key = mapKey;
        }

        private async Task<string> GetNewKey()
        {
            var window = (WritingWindow)_windowsHandler.Get(WindowType.Writing);
            string key;

            while (true)
            {
                key = await window.GetString();

                if (MapKeyValidator.IsValid(key))
                    break;

                window.Clear();
            }

            window.Close();
            return key;
        }
    }
}