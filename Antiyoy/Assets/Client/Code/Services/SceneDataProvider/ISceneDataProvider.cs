using ClientCode.Data.Scene;

namespace ClientCode.Services.SceneDataProvider
{
    public interface ISceneDataProvider<out T> where T : ISceneData
    {
        T Get();
    }
}