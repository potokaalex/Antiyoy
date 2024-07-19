using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectLoadState : IState
    {
        private readonly IProjectSaveLoader _saveLoader;
        private readonly IProjectStateMachine _stateMachine;
        private readonly IStaticDataProvider _staticData;

        public ProjectLoadState(IStaticDataProvider staticData, IProjectSaveLoader saveLoader, IProjectStateMachine stateMachine)
        {
            _staticData = staticData;
            _saveLoader = saveLoader;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _saveLoader.Load(out var progress);
            InitializeStaticData(progress);
            _stateMachine.SwitchTo<ProjectEnterSate>();
        }

        private void InitializeStaticData(ProjectProgressData progress)
        {
            var load = progress.Load;
            _staticData.Initialize(load.Configs, load.Prefabs);
        }
    }
}