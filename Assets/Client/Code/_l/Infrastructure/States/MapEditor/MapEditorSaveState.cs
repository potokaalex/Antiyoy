using Client.Code.Services.StateMachineCode.State;
using ClientCode.Client.Code.Services.StateMachineCode;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Map.Factory;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorSaveState : IStateSimple
    {
        private readonly StateMachine _stateMachine;
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly MapKeyFactory _mapKeyFactory;

        public MapEditorSaveState(StateMachine stateMachine, IMapSaveLoader saveLoader, ILogReceiver logReceiver, MapKeyFactory mapKeyFactory)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _mapKeyFactory = mapKeyFactory;
        }

        public async void Enter()
        {
            var mapKey = await _mapKeyFactory.Create();
            if (mapKey.Item1)
            {
                var result = await _saveLoader.Save(mapKey.Item2);

                if (result == SaveLoaderResultType.Error)
                    _logReceiver.Log(new LogData(LogType.Error, "Impossible to save: unknown reason!"));
            }

            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}