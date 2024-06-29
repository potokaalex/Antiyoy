using UnityEngine;

namespace Code
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private ConfigProvider _configProvider;
        [SerializeField] private CameraObject _camera;

        private void Awake()
        {
            var ecsFactory = new EcsFactory();
            var ecsProvider = new EcsProvider(ecsFactory);
            var cellFactory = new CellFactory(ecsProvider, _configProvider);

            ecsProvider.Initialize();
            cellFactory.Create();
            ecsFactory.Create();
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
