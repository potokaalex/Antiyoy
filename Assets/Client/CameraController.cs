using UnityEngine;

namespace Client
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField] private Camera _camera;

    public RaycastHit2D GetHitFromMousePoint()
    {
      var ray = _camera.ScreenPointToRay(Input.mousePosition);
      return Physics2D.Raycast(ray.origin, ray.direction);
    }

    private void Update()
    {
      Move();
      Scroll();
    }

    private void Scroll()
    {
      var speed = 100f;
      var size = _camera.orthographicSize;
      var delta = Input.mouseScrollDelta.y;

      if (delta > 0)
        size -= speed * Time.deltaTime;
      else if (delta < 0)
        size += speed * Time.deltaTime;

      _camera.orthographicSize = Mathf.Clamp(size, 2, 5);
    }

    private void Move()
    {
      var speed = 5f;
      var offset = speed * Time.deltaTime;

      if (Input.GetKey(KeyCode.A))
        _camera.transform.position += Vector3.left * offset;
      if (Input.GetKey(KeyCode.D))
        _camera.transform.position += Vector3.right * offset;

      if (Input.GetKey(KeyCode.W))
        _camera.transform.position += Vector3.up * offset;
      if (Input.GetKey(KeyCode.S))
        _camera.transform.position += Vector3.down * offset;
    }
  }
}