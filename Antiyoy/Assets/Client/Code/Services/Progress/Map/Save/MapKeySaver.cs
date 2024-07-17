using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using ClientCode.UI.Factory;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.Services.Progress.Map.Save
{
    public class MapKeySaver : IProgressWriter<MapProgressData>
    {
        private readonly WindowsFactory _windowsFactory;
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;

        public MapKeySaver(WindowsFactory windowsFactory, IMapSaveLoader saveLoader, ILogReceiver logReceiver)
        {
            _windowsFactory = windowsFactory;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
        }

        public async Task OnSave(MapProgressData progress)
        {
            var mapKey = progress.Key;

            if (string.IsNullOrEmpty(mapKey))
                mapKey = await GetNewKey();

            progress.Key = mapKey;
        }

        private async Task<string> GetNewKey()
        {
            var window = (WritingWindow)_windowsFactory.Get(WindowType.Writing);
            window.Open();

            var key = await window.GetString();
            var validatorResult = _saveLoader.IsKeyValidToSaveWithoutOverwrite(key);

            if (validatorResult == SaveLoaderResultType.ErrorFileIsExist)
                _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: this key is already exist!"));
            else if (validatorResult == SaveLoaderResultType.ErrorFileNameIsEmptyOrNull)
                _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: this key is empty or null!"));
            else if (validatorResult == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: unknown reason!"));

            window.Clear();
            window.Close();
            return key;
        }
    }
}