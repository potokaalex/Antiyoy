using UnityEngine;
using Zenject;

namespace Client.Code.Services.Config
{
    public class ConfigsController : IInitializable, IProvider<ConfigData>
    {
        private ConfigData _configData;

        ConfigData IProvider<ConfigData>.Value => _configData;
        
        public void Initialize() => _configData = Resources.Load<ConfigData>("ConfigData");
    }
}