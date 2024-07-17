using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorSaveState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;

        public MapEditorSaveState(IStateMachine stateMachine, IMapSaveLoader saveLoader, ILogReceiver logReceiver)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
        }

        public async void Enter()
        {
            var result = await _saveLoader.Save();

            if (result == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to save: unknown reason!"));

            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}