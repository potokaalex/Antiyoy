using ClientCode.Data.Progress;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.StateMachine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorEnterState : IState
    {
        private readonly CellFactory _cellFactory;
        private readonly EcsFactory _ecsFactory;
        private readonly TileFactory _tileFactory;
        private readonly IStateMachine _stateMachine;

        public MapEditorEnterState(CellFactory cellFactory, EcsFactory ecsFactory, TileFactory tileFactory, IStateMachine stateMachine)
        {
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            //загрузить мир (в прогресс) -> создать мир
            _ecsFactory.Create();
            
            _cellFactory.Initialize();
            _cellFactory.Create(new MapProgressData());
            
            _tileFactory.Initialize();
            
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}