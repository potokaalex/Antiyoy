using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorSaveState : IState
    {
        private readonly IWindowsHandler _windowsHandler;
        private readonly IStateMachine _stateMachine;
        private readonly IProgressDataSaveLoader _saveLoader;

        public MapEditorSaveState(IWindowsHandler windowsHandler,  IStateMachine stateMachine, IProgressDataSaveLoader saveLoader)
        {
            _windowsHandler = windowsHandler;
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
        }

        public async void Enter()
        {
            await SaveProgress();
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }

        private async Task SaveProgress()
        {
            var progress = _saveLoader.LoadPlayer();

            if (string.IsNullOrEmpty(progress.Map.Key))
            {
                await SaveWithNewMapKey(progress);
                return;
            }

            _saveLoader.SavePlayer();
        }

        private async Task SaveWithNewMapKey(PlayerProgressData progress)
        {
            var window = (WritingWindow)_windowsHandler.Get(WindowType.Writing);

            while (true)
            {
                var mapKey = await window.GetString();
                progress.Map.Key = mapKey;
                    
                if ( _saveLoader.SavePlayer())
                    break;
                    
                window.Clear();
            }
            
            window.Close();
        }
    }
}