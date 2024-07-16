using ClientCode.Infrastructure.States.MainMenu;
using ClientCode.Services.CanvasService;
using ClientCode.Services.StateMachine;
using ClientCode.Services.Updater;
using ClientCode.UI.Factory;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectEnterSate : IState
    {
        private readonly ProjectCanvasController _projectCanvasController;
        private readonly UIFactory _uiFactory;
        private readonly IStateMachine _stateMachine;
        private readonly IUpdater _updater;

        public ProjectEnterSate(ProjectCanvasController projectCanvasController, UIFactory uiFactory, IStateMachine stateMachine,
            IUpdater updater)
        {
            _projectCanvasController = projectCanvasController;
            _uiFactory = uiFactory;
            _stateMachine = stateMachine;
            _updater = updater;
        }

        public void Enter()
        {
            _updater.OnProjectExit += () => _stateMachine.SwitchTo<ProjectExitState>();
            _projectCanvasController.Initialize();
            _uiFactory.Initialize(_projectCanvasController.DefaultElementsRoot);
            _stateMachine.SwitchTo<MainMenuLoadState>();
        }
    }
}