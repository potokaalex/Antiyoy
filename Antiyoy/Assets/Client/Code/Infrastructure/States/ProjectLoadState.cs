using ClientCode.Data.Progress.Load;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class ProjectLoadState : IState
    {
        private readonly IProgressDataProvider _progressDataProvider;
        private readonly IStateMachine _stateMachine;
        private readonly IStaticDataProvider _staticDataProvider;

        public ProjectLoadState(IStaticDataProvider staticDataProvider, IProgressDataProvider progressDataProvider, IStateMachine stateMachine)
        {
            _staticDataProvider = staticDataProvider;
            _progressDataProvider = progressDataProvider;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            InitializeStaticData();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }

        private void InitializeStaticData()
        {
            var loadData = (ProjectLoadData)_progressDataProvider.Load;
            _staticDataProvider.Initialize(loadData.Configs, loadData.Prefabs);
        }
    }
}