using Client.Code.Services;
using Client.Code.Services.Config;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace ClientCode.Services.SceneLoader
{
    public class SceneLoader
    {
        private readonly IProvider<ConfigData> _configData;

        public SceneLoader(IProvider<ConfigData> configData) => _configData = configData;

        public void LoadScene(SceneName name) => LoadSceneAsync(name).Forget();

        public async UniTask LoadSceneAsync(SceneName name)
        {
            var nameStr = _configData.Value.Scenes[name];
            await SceneManager.LoadSceneAsync(nameStr, LoadSceneMode.Single).ToUniTask();
            await UniTask.Yield();
        }
    }
}