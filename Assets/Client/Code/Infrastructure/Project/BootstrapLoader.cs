using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientCode.Client.Code.Bootstrap
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        private static int _loadingsCount;

        public void Awake()
        {
            if (FindObjectOfType<Bootstrapper>())
                return;

            var bootSceneIndex = 0;

            if (_loadingsCount > 0)
                throw new Exception($"Cant find {nameof(Bootstrapper)} on scene with index {bootSceneIndex}");
            _loadingsCount++;

            foreach (var m in FindObjectsOfType<MonoBehaviour>()) 
                Destroy(m);

            SceneManager.LoadScene(bootSceneIndex);
        }
#endif
    }
}