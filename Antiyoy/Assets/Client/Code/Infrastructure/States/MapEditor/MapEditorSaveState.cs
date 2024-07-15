using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorSaveState : IState
    {
        private readonly IProgressDataProvider _progressDataProvider;
        private readonly IWindowsHandler _windowsHandler;
        private readonly IProgressDataSaveLoader _progressDataSaveLoader;
        private readonly IStateMachine _stateMachine;

        public MapEditorSaveState(IProgressDataProvider progressDataProvider, IWindowsHandler windowsHandler,
            IProgressDataSaveLoader progressDataSaveLoader, IStateMachine stateMachine)
        {
            _progressDataProvider = progressDataProvider;
            _windowsHandler = windowsHandler;
            _progressDataSaveLoader = progressDataSaveLoader;
            _stateMachine = stateMachine;
        }

        public async void Enter()
        {
            await SaveProgress();
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }

        private async Task SaveProgress()
        {
            var progress = _progressDataProvider.MapEditor;

            if (string.IsNullOrEmpty(progress.Map.Key))
            {
                await SaveWithNewMapKey(progress);
                return;
            }

            _progressDataSaveLoader.SaveMapEditor(progress);
        }

        private async Task SaveWithNewMapKey(MapEditorProgressData progress)
        {
            var window = (WritingWindow)_windowsHandler.Get(WindowType.Writing);

            while (true)
            {
                var mapKey = await window.GetString();
                progress.Map.Key = mapKey;
                    
                if (_progressDataSaveLoader.SaveMapEditor(progress))
                    break;
                    
                window.Clear();
            }
            
            window.Close();
        }
    }
}