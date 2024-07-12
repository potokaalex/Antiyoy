namespace ClientCode.Services.SceneDataProvider
{
    public interface ISceneDataProvider<out T> where T : ISceneData
    {
        public T Get();        
    }
}