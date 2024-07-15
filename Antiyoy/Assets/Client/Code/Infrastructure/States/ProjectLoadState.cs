using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Infrastructure.States.MainMenu;
using ClientCode.Services.Progress;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class ProjectLoadState : IState, IProgressReader
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
            _saveLoader.RegisterActor(this);
            _saveLoader.Load();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }

        public void Exit() => _saveLoader.UnRegisterActor(this);

        public Task OnLoad(ProgressData progress)
        {
            var load = progress.Project.Load;
            _staticDataProvider.Initialize(load.Configs, load.Prefabs);
            return Task.CompletedTask;
        }
    }
}