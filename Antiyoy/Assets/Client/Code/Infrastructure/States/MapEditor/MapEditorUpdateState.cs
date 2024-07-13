using ClientCode.Data.Scene;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.SceneDataProvider;
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
        private readonly ISceneDataProvider<MapEditorSceneData> _sceneDataProvider;
        private IEcsSystems _ecsSystems;
        private MapEditorSceneData _sceneData;

        public MapEditorUpdateState(IUpdater updater, IEcsProvider ecsProvider, TileFactory tileFactory,
            ISceneDataProvider<MapEditorSceneData> sceneDataProvider)
        {
            _updater = updater;
            _ecsProvider = ecsProvider;
            _tileFactory = tileFactory;
            _sceneDataProvider = sceneDataProvider;
        }

        public void Enter()
        {
            _sceneData = _sceneDataProvider.Get();
            _ecsSystems = _ecsProvider.GetSystems();
            _ecsSystems.Init();

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
            if(_sceneData.EventSystem.IsPointerOverGameObject())
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