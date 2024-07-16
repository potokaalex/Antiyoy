using ClientCode.Services.Progress;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuEnterState : IState
    {
        private readonly IProgressDataSaveLoader _saveLoader;

        public MainMenuEnterState(IProgressDataSaveLoader saveLoader) => _saveLoader = saveLoader;

        public void Enter() => _saveLoader.Load();

        public void Exit() => _saveLoader.Save();
    }
}