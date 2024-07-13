using ClientCode.Data.Progress;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Startup
{
    public class MapEditorStartup : MonoBehaviour
    {
        [SerializeField] private CameraObject _camera;
        private IEcsProvider _ecsProvider;
        private CellFactory _cellFactory;
        private EcsFactory _ecsFactory;
        private TileFactory _tileFactory;
        private IEcsSystems _ecsSystems;
        private bool _isCreateTileMode;

        [Inject]
        private void Constructor(IEcsProvider ecsProvider, CellFactory cellFactory, EcsFactory ecsFactory,
            TileFactory tileFactory)
        {
            _ecsProvider = ecsProvider;
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
            _tileFactory = tileFactory;
        }

        private void Awake()
        {
            _ecsFactory.Create();
            _cellFactory.Initialize();
            _cellFactory.Create(new MapProgressData());
            _ecsSystems = _ecsProvider.GetSystems();

            _tileFactory.Initialize();
        }

        private void Start() => _ecsSystems.Init();

        private void FixedUpdate() => _ecsSystems.Run();

        private void Update() => TouchCell();

        private void OnDestroy() => _ecsFactory.Destroy();

        private void TouchCell()
        {
            var ray = _camera.GetRayFromCurrentMousePosition();
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