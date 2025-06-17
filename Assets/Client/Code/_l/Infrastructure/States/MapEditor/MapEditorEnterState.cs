using Client.Code.Services.StateMachineCode.State;
using ClientCode.Client.Code.Services.StateMachineCode;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorEnterState : IStateSimple
    {
        private readonly CellFactory _cellFactory;
        private readonly IEcsProvider _ecsProvider;
        private readonly StateMachine _stateMachine;
        private readonly TileFactory _tileFactory;
        private readonly RegionFactory _regionFactory;

        public MapEditorEnterState(CellFactory cellFactory, TileFactory tileFactory, StateMachine stateMachine, IEcsProvider ecsProvider,
            RegionFactory regionFactory)
        {
            _cellFactory = cellFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
            _ecsProvider = ecsProvider;
            _regionFactory = regionFactory;
        }

        public void Enter()
        {
            var cells = _cellFactory.Create();
            _tileFactory.Create(cells);
            _regionFactory.Create(cells);

            _ecsProvider.GetSystems().Init();//!
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}