using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Bootstrap
{
    public class BootstrapLoader : MonoBehaviour
    {
        [SerializeField] private ScenesConfig _scenesConfig;
        
#if UNITY_EDITOR
        private void Start() => SceneManager.LoadScene(_scenesConfig.BootstrapSceneName);
#endif
    }
}