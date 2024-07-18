using ClientCode.Services.Progress.Project;
using ClientCode.Services.StateMachine;
using ClientCode.Services.Updater;
using UnityEditor;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectExitState : IState
    {
        private readonly IProjectSaveLoader _progressDataSaveLoader;
        private readonly IUpdater _updater;

        public ProjectExitState(IProjectSaveLoader progressDataSaveLoader, IUpdater updater)
        {
            _progressDataSaveLoader = progressDataSaveLoader;
            _updater = updater;
        }

        public void Enter()
        {
            _updater.ClearAllListeners();
            _progressDataSaveLoader.Save();
            Quit();
        }

        private void Quit()
#if UNITY_EDITOR
            => EditorApplication.isPlaying = false;
#else
            => UnityEngine.Application.Quit();
#endif
    }
}