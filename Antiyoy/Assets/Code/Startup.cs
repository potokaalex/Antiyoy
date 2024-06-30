using System;
using Code.Cell;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Code
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private CameraObject _camera;
        private EcsProvider _ecsProvider;
        private CellFactory _cellFactory;
        private EcsFactory _ecsFactory;
        private IEcsSystems _ecsSystems;
        private bool _isCreateCellMode;

        [Inject]
        private void Constructor(EcsProvider ecsProvider, CellFactory cellFactory, EcsFactory ecsFactory)
        {
            _ecsProvider = ecsProvider;
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
        }
        
        private void Awake()
        {
            _ecsFactory.Create();
            _cellFactory.Create();
            _ecsSystems = _ecsProvider.GetSystems();
        }

        private void Start() => _ecsSystems.Init();

        private void FixedUpdate() => _ecsSystems.Run();

        private void Update() => TouchCell();

        private void OnDestroy() => _ecsFactory.Destroy();

        //мы должны создать псевдо-клетки, только для того, чтобы получить индекс, для создания/удаления настоящей клетки!
        //создать новую систему (т.к. создание мнимымых клеток подразумевается только здесь.
        //расширить старую систему ?
        private void TouchCell()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            
            var ray = _camera.GetRayFromCurrentMousePosition();
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            //hmm...
            if (hit.transform)
            {
                if(hit.transform.TryGetComponent<CellObject>(out var cell))
                    Debug.Log(cell.Index);
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 100), "CreateCell"))
                _isCreateCellMode = true;
            //if()
        }
    }
}
