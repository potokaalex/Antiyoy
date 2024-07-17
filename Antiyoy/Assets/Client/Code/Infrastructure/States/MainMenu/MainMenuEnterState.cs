using ClientCode.Services.Progress.Project;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuEnterState : IState
    {
        private readonly IProjectSaveLoader _saveLoader;

        public MainMenuEnterState(IProjectSaveLoader saveLoader) => _saveLoader = saveLoader;

        public void Enter() => _saveLoader.Load();

        public void Exit() => _saveLoader.Save();
    }
}