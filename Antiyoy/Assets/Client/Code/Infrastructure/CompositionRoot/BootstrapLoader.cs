using ClientCode.Data.Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientCode.Infrastructure.CompositionRoot
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SceneConfig _sceneConfig;
        
        private void Awake() => SceneManager.LoadScene(_sceneConfig.BootstrapSceneName);
#endif
    }
}