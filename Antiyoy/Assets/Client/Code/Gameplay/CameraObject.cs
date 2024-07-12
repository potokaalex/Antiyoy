using UnityEngine;

namespace Code.Gameplay
{
    public class CameraObject : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        private void Update()
        {
            Move();
            Scroll();
        }

        public Ray GetRayFromCurrentMousePosition() => _camera.ScreenPointToRay(Input.mousePosition);
        
        private void Scroll()
        {
            var speed = 100f;
            var newSize = _camera.orthographicSize;
            
            if (Input.mouseScrollDelta.y > 0)
                newSize -= speed * Time.deltaTime;
            
            if (Input.mouseScrollDelta.y < 0)
                newSize += speed * Time.deltaTime;

            _camera.orthographicSize = Mathf.Clamp(newSize, 1, 5);
        }

        private void Move()
        {
            var speed = 5f;
            
            if (Input.GetKey(KeyCode.A))
                transform.position += Vector3.left * (speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                transform.position += Vector3.right * (speed * Time.deltaTime);
            
            if (Input.GetKey(KeyCode.W))
                transform.position += Vector3.up * (speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S))
                transform.position += Vector3.down * (speed * Time.deltaTime);
        }
    }
}
