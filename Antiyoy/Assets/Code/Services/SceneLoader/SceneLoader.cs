using UnityEngine.SceneManagement;

namespace Code.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public void LoadSceneAsync(string sceneName, ISceneLoaderScreen loaderScreen = null)
        {
            loaderScreen?.Show();
            
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single)!.completed +=
                _ => loaderScreen?.Hide();
        }
    }
}