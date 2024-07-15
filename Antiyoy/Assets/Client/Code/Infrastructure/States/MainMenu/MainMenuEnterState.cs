using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MainMenu
{
    public class MainMenuEnterState : IState
    {
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;

        public MainMenuEnterState(IProgressDataSaveLoader saveLoader, ILogReceiver logReceiver)
        {
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
        }

        public void Enter()
        {
            _saveLoader.Load();
            _logReceiver.Log(new LogData { Type = LogType.Error, Message = "SomeLog!" });
        }

        public void Exit() => _saveLoader.Save();
    }
}