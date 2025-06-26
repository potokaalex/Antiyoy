using UnityEngine;
using Zenject;

namespace Client.Code.Services.Config
{
    public class ConfigsController : IInitializable, IConfigsProvider
    {
        private ConfigData _configData;

        ConfigData IConfigsProvider.Data => _configData;

        public void Initialize() => _configData = Resources.Load<ConfigData>("ConfigData");
    }
}