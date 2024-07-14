using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class MainMenuLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly IProgressDataProvider _progressDataProvider;

        public MainMenuLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider, IProgressDataSaveLoader saveLoader,
            IProgressDataProvider progressDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
            _saveLoader = saveLoader;
            _progressDataProvider = progressDataProvider;
        }

        public void Enter()
        {
            _progressDataProvider.MainMenu = _saveLoader.LoadMainMenu();
            LoadScene();
        }

        private void LoadScene()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MainMenuSceneName);
        }
    }
}