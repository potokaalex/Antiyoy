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
        private readonly IEcsProvider _ecsProvider;
        private readonly StateMachine _stateMachine;
        private readonly TileFactoryOld _tileFactoryOld;
        private readonly RegionFactory _regionFactory;

        public MapEditorEnterState(TileFactoryOld tileFactoryOld, StateMachine stateMachine, IEcsProvider ecsProvider,
            RegionFactory regionFactory)
        {
            _tileFactoryOld = tileFactoryOld;
            _stateMachine = stateMachine;
            _ecsProvider = ecsProvider;
            _regionFactory = regionFactory;
        }

        public void Enter()
        {
            var cells = new int[0];// _cellFactory.Create();
            _tileFactoryOld.Create(cells);
            _regionFactory.Create(cells);

            _ecsProvider.GetSystems().Init();//!
            _stateMachine.SwitchTo<MapEditorUpdateState>();
        }
    }
}