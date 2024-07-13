using ClientCode.Data.Static;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientCode.Utilities
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SceneConfig _sceneConfig;

        private void Awake()
        {
            if (SceneManager.GetActiveScene().name != _sceneConfig.BootstrapSceneName)
                SceneManager.LoadScene(_sceneConfig.BootstrapSceneName);
        }
#endif
    }
}