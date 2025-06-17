using Client.Code.Services.StateMachineCode.State;
using ClientCode.Services.Progress.Project;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuState : IStateSimple
    {
        private readonly IProjectSaveLoader _saveLoader;

        public MainMenuState(IProjectSaveLoader saveLoader) => _saveLoader = saveLoader;

        public void Enter()
        {
        }

        public void Exit() => _saveLoader.Save();
    }
}