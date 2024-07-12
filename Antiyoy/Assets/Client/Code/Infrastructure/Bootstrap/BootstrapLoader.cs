using ClientCode.Data.Configs;
using ClientCode.Services.StaticDataProvider;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ClientCode.Infrastructure.Bootstrap
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        private StaticDataProvider _staticDataProvider;

        [Inject]
        private void Construct(StaticDataProvider staticDataProvider) => _staticDataProvider = staticDataProvider;

        private void Awake()
        {
            var scenesConfig = _staticDataProvider.Get<SceneConfig>();
            SceneManager.LoadScene(scenesConfig.BootstrapSceneName);
        }
#endif
    }
}