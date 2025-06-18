using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientCode.Client.Code.Bootstrap
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        public void Awake()
        {
            if (!FindObjectOfType<Bootstrapper>())
            {
                foreach (var m in FindObjectsOfType<MonoBehaviour>())
                    DestroyImmediate(m);
                SceneManager.LoadScene(0);
            }
        }
#endif
    }
}