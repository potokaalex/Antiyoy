using Client.Code.Services.Config;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Client.Code.Services.Scene
{
    public class SceneLoader
    {
        private readonly IConfigsProvider _configsProvider;

        public SceneLoader(IConfigsProvider configsProvider) => _configsProvider = configsProvider;

        public void LoadScene(SceneName name) => LoadSceneAsync(name).Forget();

        public async UniTask LoadSceneAsync(SceneName name)
        {
            var nameStr = _configsProvider.Data.Scenes[name];
            await SceneManager.LoadSceneAsync(nameStr, LoadSceneMode.Single).ToUniTask();
            await UniTask.Yield();
        }
    }
}