using Cysharp.Threading.Tasks;

namespace ClientCode.Services.SceneLoader
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName);
        T FindInScene<T>(string sceneName);
    }
}