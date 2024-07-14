using ClientCode.Data.Scene;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.StateMachine;
using ClientCode.Services.Updater;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorUpdateState : IState
    {
        private readonly IUpdater _updater;
        private readonly IEcsProvider _ecsProvider;
        private readonly TileFactory _tileFactory;
        private readonly MapEditorSceneData _sceneData;
        private IEcsSystems _ecsSystems;

        public MapEditorUpdateState(IUpdater updater, IEcsProvider ecsProvider, TileFactory tileFactory, MapEditorSceneData sceneData)
        {
            _updater = updater;
            _ecsProvider = ecsProvider;
            _tileFactory = tileFactory;
            _sceneData = sceneData;
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

        private void Update() => TouchCell();

        private void FixedUpdate() => _ecsSystems.Run();

        private void TouchCell()
        {
            if (_sceneData.EventSystem.IsPointerOverGameObject())
                return;

            var ray = _sceneData.Camera.GetRayFromCurrentMousePosition();
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.transform && hit.transform.TryGetComponent<CellObject>(out var cell))
            {
                if (Input.GetMouseButton(0))
                    _tileFactory.Create(cell);
                else if (Input.GetMouseButton(1))
                    _tileFactory.Destroy(cell);
            }
        }
    }
}