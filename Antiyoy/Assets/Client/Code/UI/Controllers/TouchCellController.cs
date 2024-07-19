using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClientCode.UI.Controllers
{
    public abstract class TouchCellController
    {
        private readonly EventSystem _eventSystem;
        private readonly CameraController _camera;

        private protected TouchCellController(EventSystem eventSystem, CameraController camera)
        {
            _eventSystem = eventSystem;
            _camera = camera;
        }

        public void Update()
        {
            if (_eventSystem.IsPointerOverGameObject())
                return;

            var ray = _camera.GetRayFromCurrentMousePosition();
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.transform && hit.transform.TryGetComponent<CellObject>(out var cell)) 
                OnCellTouch(cell);
        }

        private protected abstract void OnCellTouch(CellObject cell);
    }
}