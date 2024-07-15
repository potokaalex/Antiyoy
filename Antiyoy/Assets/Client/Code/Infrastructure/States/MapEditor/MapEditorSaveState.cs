using ClientCode.Services.Progress;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorSaveState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IProgressDataSaveLoader _saveLoader;

        public MapEditorSaveState(IStateMachine stateMachine, IProgressDataSaveLoader saveLoader)
        {
            _stateMachine = stateMachine;
            _saveLoader = saveLoader;
        }

        public async void Enter()
        {
            await _saveLoader.SavePlayer();
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}