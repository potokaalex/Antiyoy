using UnityEngine;

namespace ClientCode.Services.InputService
{
    public class InputService : MonoBehaviour, IInputService
    {
        private bool _leftMouseButtonDown;
        private bool _rightMouseButtonDown;

        public bool IsMouseButtonDown(MouseType type)
        {
            if (type == MouseType.Left)
                return _leftMouseButtonDown;
            if (type == MouseType.Right)
                return _rightMouseButtonDown;

            return false;
        }

        private void Update()
        {
            _leftMouseButtonDown = Input.GetMouseButton(0);
            _rightMouseButtonDown = Input.GetMouseButton(1);
        }
    }
}