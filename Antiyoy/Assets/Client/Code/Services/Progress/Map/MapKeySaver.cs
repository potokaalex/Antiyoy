using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.Services.Progress.Map
{
    public class MapKeySaver : IProgressWriter
    {
        private readonly WindowsFactory _windowsFactory;
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;

        public MapKeySaver(WindowsFactory windowsFactory, IProgressDataSaveLoader saveLoader, ILogReceiver logReceiver)
        {
            _windowsFactory = windowsFactory;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
        }

        public async Task OnSave(ProgressData progress)
        {
            var mapKey = progress.Player.Map.Key;

            if (string.IsNullOrEmpty(mapKey))
                mapKey = await GetNewKey();

            progress.Player.Map.Key = mapKey;
        }

        private async Task<string> GetNewKey()
        {
            var window = (WritingWindow)_windowsFactory.Get(WindowType.Writing);
            string key;

            while (true)
            {
                key = await window.GetString();
                var validatorResult = _saveLoader.IsMapKeyValidToSaveWithoutOverwrite(key);

                if (validatorResult == SaveLoaderResultType.Normal)
                    break;

                if (validatorResult == SaveLoaderResultType.ErrorFileIsExist)
                    _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: this key is already exist!"));
                else if (validatorResult == SaveLoaderResultType.Error)
                    _logReceiver.Log(new LogData(LogType.Error, "Not valid map key: unknown reason!"));
                
                window.Clear();
            }

            window.Close();
            return key;
        }
    }
}