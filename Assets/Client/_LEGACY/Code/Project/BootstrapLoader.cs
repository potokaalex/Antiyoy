using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Code.Project
{
  public class BootstrapLoader : MonoBehaviour
  {
#if UNITY_EDITOR
    public void Awake()
    {
      if (!FindObjectsByType<BootstrapLoader>(FindObjectsSortMode.None).Except(new[] { this }).Any())
      {
        var bootSceneBindIndex = 0;

        if (SceneManager.GetActiveScene().buildIndex != bootSceneBindIndex)
        {
          foreach (var m in FindObjectsByType<Behaviour>(FindObjectsSortMode.None))
            if (m != this)
              m.gameObject.SetActive(false);

          SceneManager.LoadScene(bootSceneBindIndex);
        }

        transform.SetParent(null);
        DontDestroyOnLoad(this);
      }
    }
#endif
  }
}