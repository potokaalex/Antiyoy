using ClientCode.Services.Progress;
using ClientCode.Services.StateMachine;
using ClientCode.Services.Updater;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectExitState : IState
    {
        private readonly IProgressDataSaveLoader _progressDataSaveLoader;
        private readonly IUpdater _updater;

        public ProjectExitState(IProgressDataSaveLoader progressDataSaveLoader, IUpdater updater)
        {
            _progressDataSaveLoader = progressDataSaveLoader;
            _updater = updater;
        }

        public void Enter()
        {
            _updater.ClearAllListeners();
            _progressDataSaveLoader.Save();
        }
    }
}