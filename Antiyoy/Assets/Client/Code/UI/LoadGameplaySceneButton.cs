using ClientCode.Data.Configs;
using ClientCode.Infrastructure;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StaticDataProvider;
using Zenject;

namespace ClientCode.UI
{
    public class LoadGameplaySceneButton : ButtonBase
    {
        private ISceneLoader _sceneLoader;
        private StaticDataProvider _staticDataProvider;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, StaticDataProvider staticDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
        }

        private protected override void OnClick()
        {
            var scenesConfig = _staticDataProvider.Get<SceneConfig>();
            _sceneLoader.LoadSceneAsync(scenesConfig.GameplaySceneName);
        }
    }
}