using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class ProjectLoadState : IState
    {
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly IStateMachine _stateMachine;
        private readonly IProgressDataProvider _progressDataProvider;
        private readonly IStaticDataProvider _staticDataProvider;

        public ProjectLoadState(IStaticDataProvider staticDataProvider, IProgressDataSaveLoader saveLoader, IStateMachine stateMachine,
            IProgressDataProvider progressDataProvider)
        {
            _staticDataProvider = staticDataProvider;
            _saveLoader = saveLoader;
            _stateMachine = stateMachine;
            _progressDataProvider = progressDataProvider;
        }

        public void Enter()
        {
            LoadProgress();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }

        private void LoadProgress()
        {
            var projectData = _saveLoader.LoadProject();
            _progressDataProvider.Project = projectData;

            _staticDataProvider.Initialize(projectData.Load.Configs, projectData.Load.Prefabs);
        }
    }
}