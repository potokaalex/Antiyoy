using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Bootstrap
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        private ConfigProvider _configProvider;

        [Inject]
        private void Construct(ConfigProvider configProvider) => _configProvider = configProvider;

        private void Awake()
        {
            var scenesConfig = _configProvider.GetScenes();
            SceneManager.LoadScene(scenesConfig.BootstrapSceneName);
        }
#endif
    }
}