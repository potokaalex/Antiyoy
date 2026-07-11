using UnityEngine;

namespace Client
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField] private Camera _camera;
    private Vector3 _positionVelocity;

    public RaycastHit2D GetHitFromMousePoint()
    {
      var ray = _camera.ScreenPointToRay(Input.mousePosition);
      return Physics2D.Raycast(ray.origin, ray.direction);
    }

    public bool GetHitFromMousePoint(out RaycastHit2D hit)
    {
      var ray = _camera.ScreenPointToRay(Input.mousePosition);
      hit = Physics2D.Raycast(ray.origin, ray.direction);
      return hit;
    }

    private void Update()
    {
#if UNITY_EDITOR
      Move();
      Scroll();
#else
      MobileMove();
      MobileScroll();
#endif
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

    private void MobileScroll()
    {
      if (Input.touchCount == 2)
      {
        var speed = 20f;
        var size = _camera.orthographicSize;
        var touch0 = Input.GetTouch(0);
        var touch1 = Input.GetTouch(1);
        var prevPos0 = touch0.position - touch0.deltaPosition;
        var prevPos1 = touch1.position - touch1.deltaPosition;
        var prevDistance = Vector2.Distance(prevPos0, prevPos1);
        var currentDistance = Vector2.Distance(touch0.position, touch1.position);
        var delta = currentDistance - prevDistance;

        if (delta > 0)
          size -= speed * Time.deltaTime;
        else if (delta < 0)
          size += speed * Time.deltaTime;

        _camera.orthographicSize = Mathf.Clamp(size, 2, 5);
      }
    }

    private void MobileMove()
    {
      if (Input.touchCount == 1)
      {
        var normalizationFactor = Mathf.Max(Screen.width, Screen.height);
        var normalizedPositionDelta = -Input.mousePositionDelta / normalizationFactor;
        var targetVelocity = Input.GetMouseButton(0) ? normalizedPositionDelta * 1500f : Vector3.zero;
        _positionVelocity = Vector3.Lerp(_positionVelocity, targetVelocity, 20f * Time.deltaTime);
        _positionVelocity = Vector3.ClampMagnitude(_positionVelocity, 15);
        var pos = _camera.transform.position + _positionVelocity * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -2, 8);
        pos.y = Mathf.Clamp(pos.y, -2, 8);
        _camera.transform.position = pos;
      }
    }
  }
}