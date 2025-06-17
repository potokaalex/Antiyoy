using ClientCode.Gameplay;
using ClientCode.Gameplay.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClientCode.UI.Controllers
{
    public abstract class TouchCellController
    {
        private readonly EventSystem _eventSystem;
        private readonly CameraController _camera;
        private readonly GridManager _gridManager;
        private readonly IEcsProvider _ecsProvider;

        private protected TouchCellController(EventSystem eventSystem, CameraController camera, GridManager gridManager)
        {
            _eventSystem = eventSystem;
            _camera = camera;
            _gridManager = gridManager;
        }

        public void Update()
        {
            if (_eventSystem.IsPointerOverGameObject())
                return;

            var ray = _camera.GetRayFromCurrentMousePosition();
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.transform && _gridManager.GetCellEntity(hit.point, out var cell))
                OnCellTouch(cell);
        }

        private protected abstract void OnCellTouch(int cell);
    }
}