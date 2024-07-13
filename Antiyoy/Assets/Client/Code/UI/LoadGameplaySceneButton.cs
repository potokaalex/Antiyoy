using ClientCode.Services.SceneLoader;
using ClientCode.Services.StaticDataProvider;
using Zenject;

namespace ClientCode.UI
{
    public class LoadGameplaySceneButton : ButtonBase
    {
        private ISceneLoader _sceneLoader;
        private IStaticDataProvider _staticDataProvider;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
        }

        private protected override void OnClick()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.GameplaySceneName);
        }
    }
}