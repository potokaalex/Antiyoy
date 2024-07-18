using ClientCode.Services.Progress.Project;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuState : IState
    {
        private readonly IProjectSaveLoader _saveLoader;

        public MainMenuState(IProjectSaveLoader saveLoader) => _saveLoader = saveLoader;

        public void Enter()
        {
        }

        public void Exit() => _saveLoader.Save();
    }
}