using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticData;
        private readonly IProjectSaveLoader _projectSaveLoader;
        private readonly IProjectStateMachine _projectStateMachine;

        public MainMenuLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticData, IProjectSaveLoader projectSaveLoader,
            IProjectStateMachine projectStateMachine)
        {
            _sceneLoader = sceneLoader;
            _staticData = staticData;
            _projectSaveLoader = projectSaveLoader;
            _projectStateMachine = projectStateMachine;
        }

        public async void Enter()
        {
            var scenesConfig = _staticData.Configs.Scene;
            await _sceneLoader.LoadSceneAsync(scenesConfig.MainMenuSceneName);
            _projectSaveLoader.Load();
            _projectStateMachine.SwitchTo<MainMenuState>();
        }
    }
}