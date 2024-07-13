using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;

        public MapEditorLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
        }

        public void Enter()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MapEditorSceneName);
        }
    }
}