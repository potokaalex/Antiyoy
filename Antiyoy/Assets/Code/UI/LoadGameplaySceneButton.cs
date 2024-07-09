using Code.Infrastructure;
using Code.Services.SceneLoader;
using Zenject;

namespace Code.UI
{
    public class LoadGameplaySceneButton : ButtonBase
    {
        private ISceneLoader _sceneLoader;
        private ConfigProvider _configProvider;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, ConfigProvider configProvider)
        {
            _sceneLoader = sceneLoader;
            _configProvider = configProvider;
        }

        private protected override void OnClick()
        {
            var scenesConfig = _configProvider.GetScenes();
            _sceneLoader.LoadSceneAsync(scenesConfig.GameplaySceneName);
        }
    }
}