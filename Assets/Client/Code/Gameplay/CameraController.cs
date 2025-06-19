using UnityEngine;
using Zenject;

namespace ClientCode.UI.Controllers
{
    public class CameraController : MonoBehaviour, ITickable
    {
        public Camera Camera;
        
        public void Tick()
        {
            Move();
            Scroll();
        }
        
        public RaycastHit2D GetHitFromMousePoint()
        {
            var ray = Camera.ScreenPointToRay(Input.mousePosition);
            return Physics2D.Raycast(ray.origin, ray.direction);
        }

        private void Scroll()
        {
            var speed = 100f;
            var newSize = Camera.orthographicSize;
            var delta = Input.mouseScrollDelta.y;
            
            if (delta > 0)
                newSize -= speed * Time.deltaTime;
            else if (delta < 0)
                newSize += speed * Time.deltaTime;

            Camera.orthographicSize = Mathf.Clamp(newSize, 2, 5);
        }

        private void Move()
        {
            var speed = 5f;
            var offset = speed * Time.deltaTime;
            
            if (Input.GetKey(KeyCode.A))
                Camera.transform.position += Vector3.left * offset;
            if (Input.GetKey(KeyCode.D))
                Camera.transform.position += Vector3.right * offset;

            if (Input.GetKey(KeyCode.W))
                Camera.transform.position += Vector3.up * offset;
            if (Input.GetKey(KeyCode.S))
                Camera.transform.position += Vector3.down * offset;
        }
    }
}