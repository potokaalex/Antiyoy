using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class MainMenuLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;

        public MainMenuLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
        }

        public void Enter()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MainMenuSceneName);
        }
    }
}