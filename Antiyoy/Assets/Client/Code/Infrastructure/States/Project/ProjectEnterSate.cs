using ClientCode.Infrastructure.States.MainMenu;
using ClientCode.Services.CanvasService;
using ClientCode.Services.StateMachine;
using ClientCode.Services.Updater;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectEnterSate : IState
    {
        private readonly ProjectCanvasController _projectCanvasController;
        private readonly IStateMachine _stateMachine;
        private readonly IUpdater _updater;

        public ProjectEnterSate(ProjectCanvasController projectCanvasController, IStateMachine stateMachine, IUpdater updater)
        {
            _projectCanvasController = projectCanvasController;
            _stateMachine = stateMachine;
            _updater = updater;
        }

        public void Enter()
        {
            _updater.OnProjectExit += () => _stateMachine.SwitchTo<ProjectExitState>();
            _projectCanvasController.Initialize();
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }
    }
}