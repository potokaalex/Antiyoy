using Client.Code.Services.StateMachineCode.State;
using ClientCode.Gameplay.Ecs;
using ClientCode.Services.Updater;
using ClientCode.UI.Controllers;
using Leopotam.EcsLite;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorUpdateState : IStateSimple
    {
        private readonly IUpdater _updater;
        private readonly IEcsProvider _ecsProvider;
        private readonly CameraController _camera;
        private IEcsSystems _ecsSystems;

        public MapEditorUpdateState(IUpdater updater, IEcsProvider ecsProvider, CameraController camera)
        {
            _updater = updater;
            _ecsProvider = ecsProvider;
            _camera = camera;
        }

        public void Enter()
        {
            _ecsSystems = _ecsProvider.GetSystems();
            _updater.OnUpdate += Update;
            _updater.OnFixedUpdate += FixedUpdate;
        }

        public void Exit()
        {
            _updater.OnUpdate -= Update;
            _updater.OnFixedUpdate -= FixedUpdate;
        }

        private void Update()
        {
            //_camera.Update();
            //_touchCellController.Update();
        }

        private void FixedUpdate() => _ecsSystems.Run();
    }
}