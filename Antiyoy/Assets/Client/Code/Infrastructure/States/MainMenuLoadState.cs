using ClientCode.Data.Configs;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class MainMenuLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly StaticDataProvider _staticDataProvider;

        public MainMenuLoadState(ISceneLoader sceneLoader, StaticDataProvider staticDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
        }

        public void Enter()
        {
            var scenesConfig = _staticDataProvider.Get<SceneConfig>();
            _sceneLoader.LoadSceneAsync(scenesConfig.MainMenuSceneName);
        }
    }
}