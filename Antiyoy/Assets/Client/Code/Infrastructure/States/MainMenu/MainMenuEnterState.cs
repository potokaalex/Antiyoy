using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuEnterState: IState
    {
        private readonly IProgressDataSaveLoader _saveLoader;

        public MainMenuEnterState(IProgressDataSaveLoader saveLoader) => _saveLoader = saveLoader;

        public void Enter() => _saveLoader.LoadPlayer();

        public void Exit() => _saveLoader.SavePlayer();
    }
}