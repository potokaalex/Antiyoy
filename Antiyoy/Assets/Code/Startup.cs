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

        [Inject]
        private void Constructor(EcsProvider ecsProvider, CellFactory cellFactory, EcsFactory ecsFactory)
        {
            _ecsProvider = ecsProvider;
            _cellFactory = cellFactory;
            _ecsFactory = ecsFactory;
        }
        
        private void Awake()
        {
            _ecsProvider.Initialize();
            _cellFactory.Create();
            _ecsFactory.Create();
        }

        private void Update()
        {
            TouchCell();
        }

        private void TouchCell()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            
            var ray = _camera.GetRayFromCurrentMousePosition();
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.transform)
            {
                if(hit.transform.TryGetComponent<CellObject>(out var cell))
                    Debug.Log(cell.name);
            }
        }
    }
}
