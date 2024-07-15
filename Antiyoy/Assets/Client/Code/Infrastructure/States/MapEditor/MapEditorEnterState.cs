using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.Progress;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorEnterState : IState
    {
        private readonly CellFactory _cellFactory;
        private readonly EcsFactory _ecsFactory;
        private readonly IEcsProvider _ecsProvider;
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly IStateMachine _stateMachine;
        private readonly TileFactory _tileFactory;

        public MapEditorEnterState(CellFactory cellFactory, EcsFactory ecsFactory, TileFactory tileFactory, IStateMachine stateMachine,
            IEcsProvider ecsProvider, IProgressDataSaveLoader saveLoader)
        {
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
            _ecsProvider = ecsProvider;
            _saveLoader = saveLoader;
        }

        public void Enter()
        {
            _ecsFactory.Create();
            _cellFactory.Initialize();
            _tileFactory.Initialize();

            _saveLoader.LoadPlayer();
            _ecsProvider.GetSystems().Init();
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}