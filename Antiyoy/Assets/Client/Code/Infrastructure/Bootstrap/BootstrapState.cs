using Code.Services.SceneLoader;
using Code.Services.StateMachine;

namespace Code.Infrastructure.Bootstrap
{
    public class BootstrapState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ConfigProvider _configProvider;

        public BootstrapState(ISceneLoader sceneLoader, ConfigProvider configProvider)
        {
            _sceneLoader = sceneLoader;
            _configProvider = configProvider;
        }

        public void Enter()
        {
            var scenesConfig = _configProvider.GetScenes();
            _sceneLoader.LoadSceneAsync(scenesConfig.MainMenuSceneName);
        }
    }
}