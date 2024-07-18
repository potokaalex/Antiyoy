using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientCode.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly List<GameObject> _sceneRootObjects = new();
        
        public async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).ToUniTask();
            await UniTask.Yield();
        }

        public T FindInScene<T>(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            scene.GetRootGameObjects(_sceneRootObjects);
                
            foreach (var rootObject in _sceneRootObjects)
                if (rootObject.TryGetComponent<T>(out var obj))
                    return obj;

            throw new Exception($"Cant find {typeof(T).Name} on {sceneName} scene");
        }
    }
}