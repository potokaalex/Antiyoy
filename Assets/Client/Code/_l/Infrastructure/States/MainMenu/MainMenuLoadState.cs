using Client.Code.Services.StateMachineCode.State;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuLoadState : IStateSimple
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticData;
        private readonly IProjectSaveLoader _projectSaveLoader;
        private readonly IProjectStateMachine _projectStateMachine;

        public MainMenuLoadState(SceneLoader sceneLoader, IStaticDataProvider staticData, IProjectSaveLoader projectSaveLoader,
            IProjectStateMachine projectStateMachine)
        {
            _sceneLoader = sceneLoader;
            _staticData = staticData;
            _projectSaveLoader = projectSaveLoader;
            _projectStateMachine = projectStateMachine;
        }

        public async void Enter()
        {
            await _sceneLoader.LoadSceneAsync(SceneName.MainMenu);
            _projectSaveLoader.Load();
            _projectStateMachine.SwitchTo<MainMenuState>();
        }
    }
}