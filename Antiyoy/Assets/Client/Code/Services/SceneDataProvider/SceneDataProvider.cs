using ClientCode.Data.Progress;
using ClientCode.Data.Scene;

namespace ClientCode.Services.SceneDataProvider
{
    public class SceneDataProvider<T> : ISceneDataProvider<T> where T : ISceneData
    {
        private readonly T _data;

        public SceneDataProvider(T data) => _data = data;

        public T Get() => _data;
    }
}