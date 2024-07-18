using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IProjectSaveLoader _projectSaveLoader;
        private readonly IProjectStateMachine _projectStateMachine;

        public MainMenuLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider, IProjectSaveLoader projectSaveLoader,
            IProjectStateMachine projectStateMachine)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
            _projectSaveLoader = projectSaveLoader;
            _projectStateMachine = projectStateMachine;
        }

        public async void Enter()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            await _sceneLoader.LoadSceneAsync(scenesConfig.MainMenuSceneName);
            _projectSaveLoader.Load();
            _projectStateMachine.SwitchTo<MainMenuState>();
        }
    }
}