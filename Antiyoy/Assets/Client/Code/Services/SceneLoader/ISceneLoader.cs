namespace ClientCode.Services.SceneLoader
{
    public interface ISceneLoader
    {
        public void LoadSceneAsync(string sceneName, ISceneLoaderScreen loaderScreen = null);
    }
}