using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class ProjectLoadState : IState
    {
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly IStateMachine _stateMachine;
        private readonly IStaticDataProvider _staticDataProvider;

        public ProjectLoadState(IStaticDataProvider staticDataProvider, IProgressDataSaveLoader saveLoader, IStateMachine stateMachine)
        {
            _staticDataProvider = staticDataProvider;
            _saveLoader = saveLoader;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            InitializeStaticData();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }

        private void InitializeStaticData()
        {
            var loadData = _saveLoader.LoadProject().Load;
            _staticDataProvider.Initialize(loadData.Configs, loadData.Prefabs);
        }
    }
}