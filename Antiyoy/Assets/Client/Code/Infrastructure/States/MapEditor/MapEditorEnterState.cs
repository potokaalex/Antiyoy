using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;
using UnityEngine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorEnterState : IState
    {
        private readonly CellFactory _cellFactory;
        private readonly EcsFactory _ecsFactory;
        private readonly IProgressDataProvider _progressDataProvider;
        private readonly IEcsProvider _ecsProvider;
        private readonly IStateMachine _stateMachine;
        private readonly TileFactory _tileFactory;

        public MapEditorEnterState(CellFactory cellFactory, EcsFactory ecsFactory, TileFactory tileFactory, IStateMachine stateMachine,
            IProgressDataProvider progressDataProvider, IEcsProvider ecsProvider)
        {
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
            _stateMachine = stateMachine;
            _progressDataProvider = progressDataProvider;
            _ecsProvider = ecsProvider;
        }

        public void Enter()
        {
            var map = _progressDataProvider.MapEditor.Map;

            _ecsFactory.Create();

            _cellFactory.Initialize();
            _cellFactory.Create(map);

            _tileFactory.Initialize();

            _ecsProvider.GetSystems().Init();
            
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}