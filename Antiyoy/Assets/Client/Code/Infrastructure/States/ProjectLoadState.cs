using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class ProjectLoadState : IState
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IStateMachine _stateMachine;

        public ProjectLoadState(IStaticDataProvider staticDataProvider, IStateMachine stateMachine)
        {
            _staticDataProvider = staticDataProvider;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            InitializeStaticData();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }

        private void InitializeStaticData()
        {
            var loadData = _staticDataProvider.ProjectLoadData;
            _staticDataProvider.Initialize(loadData.Configs, loadData.Prefabs);
        }
    }
}