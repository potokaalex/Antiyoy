using UnityEngine;
using Zenject;

namespace ClientCode.Services.InputService
{
    public class InputService : ITickable
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

        public void Tick()
        {
            _leftMouseButtonDown = Input.GetMouseButton(0);
            _rightMouseButtonDown = Input.GetMouseButton(1);
        }
    }
}