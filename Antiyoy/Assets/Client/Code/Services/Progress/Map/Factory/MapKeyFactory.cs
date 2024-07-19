using ClientCode.Data.Progress.Map;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using ClientCode.UI.Factory;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;
using Cysharp.Threading.Tasks;

namespace ClientCode.Services.Progress.Map.Factory
{
    public class MapKeyFactory : IProgressReader<MapProgressData>
    {
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly IWindowsFactory _windowsFactory;
        private MapProgressData _progress;

        public MapKeyFactory(IMapSaveLoader saveLoader, ILogReceiver logReceiver, IWindowsFactory windowsFactory)
        {
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _windowsFactory = windowsFactory;
        }

        public async UniTask<(bool, string)> Create()
        {
            if (string.IsNullOrEmpty(_progress.Key))
                return await GetNewKey();

            return (true, _progress.Key);
        }

        public void OnLoad(MapProgressData progress) => _progress = progress;

        private async UniTask<(bool, string)> GetNewKey()
        {
            var window = (IWritingWindow)_windowsFactory.Get(WindowType.Writing);
            window.Open();

            var key = await window.GetString();
            var validatorResult = _saveLoader.IsKeyValidToSaveWithoutOverwrite(key);

            if (validatorResult == SaveLoaderResultType.ErrorFileIsExist)
                _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: this key is already exist!"));
            else if (validatorResult == SaveLoaderResultType.ErrorFileNameIsEmptyOrNull)
                _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: this key is empty or null!"));
            else if (validatorResult == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: unknown reason!"));

            window.Close();
            return (validatorResult == SaveLoaderResultType.Normal, key);
        }
    }
}