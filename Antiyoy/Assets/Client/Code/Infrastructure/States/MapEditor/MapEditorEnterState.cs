using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorEnterState : IState
    {
        private readonly CellFactory _cellFactory;
        private readonly EcsFactory _ecsFactory;
        private readonly IEcsProvider _ecsProvider;
        private readonly IStateMachine _stateMachine;
        private readonly TileFactory _tileFactory;
        private readonly RegionFactory _regionFactory;

        public MapEditorEnterState(CellFactory cellFactory, EcsFactory ecsFactory, TileFactory tileFactory, IStateMachine stateMachine,
            IEcsProvider ecsProvider, RegionFactory regionFactory)
        {
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
            _ecsProvider = ecsProvider;
            _regionFactory = regionFactory;
        }

        public void Enter()
        {
            _ecsFactory.Initialize();
            _cellFactory.Initialize();
            _tileFactory.Initialize();
            _regionFactory.Initialize();

            var cells = _cellFactory.Create();
            _tileFactory.Create(cells);
            _regionFactory.Create(cells);

            _ecsProvider.GetSystems().Init();
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}