using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorEnterState : IState
    {
        private readonly CellFactory _cellFactory;
        private readonly EcsFactory _ecsFactory;
        private readonly IProgressDataProvider _progressDataProvider;
        private readonly IStateMachine _stateMachine;
        private readonly TileFactory _tileFactory;

        public MapEditorEnterState(CellFactory cellFactory, EcsFactory ecsFactory, TileFactory tileFactory, IStateMachine stateMachine,
            IProgressDataProvider progressDataProvider)
        {
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
            _progressDataProvider = progressDataProvider;
        }

        public void Enter()
        {
            var map = _progressDataProvider.Map;

            _ecsFactory.Create();

            _cellFactory.Initialize();
            _cellFactory.Create(map);

            _tileFactory.Initialize();

            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}