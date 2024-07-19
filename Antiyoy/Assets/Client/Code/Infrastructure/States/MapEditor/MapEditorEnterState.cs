using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Map.Actors;
using ClientCode.Services.Progress.Map.Factory;
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
        private readonly MapSaver _mapSaver;
        private readonly MapLoader _mapLoader;
        private readonly MapDataFactory _mapDataFactory;
        private readonly RegionFactory _regionFactory;

        public MapEditorEnterState(CellFactory cellFactory, EcsFactory ecsFactory, TileFactory tileFactory, IStateMachine stateMachine,
            IEcsProvider ecsProvider, MapSaver mapSaver, MapLoader mapLoader, MapDataFactory mapDataFactory, RegionFactory regionFactory)
        {
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
            _ecsProvider = ecsProvider;
            _mapSaver = mapSaver;
            _mapLoader = mapLoader;
            _mapDataFactory = mapDataFactory;
            _regionFactory = regionFactory;
        }

        public void Enter()
        {
            _ecsFactory.Create();
            _cellFactory.Initialize();
            _tileFactory.Initialize();
            _regionFactory.Initialize();
            
            _mapDataFactory.Initialize();
            _mapSaver.Initialize();
            _mapLoader.Initialize();

            _ecsProvider.GetSystems().Init();
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}