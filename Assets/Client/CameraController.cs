using System.Collections.Generic;
using Client.Infrastructure;
using Client.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField] private Camera _camera;
    [SerializeField] private float _positionDragMultiplier;
    [SerializeField] private float _positionLerpFactor;
    [SerializeField] private float _positionInertiaFactor;
    [SerializeField] private float _positionInertiaLerpFactor;
    [SerializeField] private float _zoomDragMultiplier;
    [SerializeField] private float _zoomLerpFactor;
    private readonly List<Touch> _touches = new();
    private readonly List<int> _ignoredTouches = new();
    private InputController _inputController;
    private Vector2? _mousePosition;
    private Vector2? _firstTouchPosition;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _inertiaTargetPosition;
    private bool _canMove;
    private float _targetSize;

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

    private void Awake()
    {
      _inputController = Locator.Get<InputController>();
      Clear();
    }

    private void Update()
    {
      CalculateTouches();
      MovePosition();
      Zoom();
    }

    private void CalculateTouches()
    {
      _touches.Clear();

      using (ListPool<int>.Get(out var allTouchesId))
      {
        for (var i = 0; i < Input.touchCount; i++)
        {
          var touch = Input.GetTouch(i);
          allTouchesId.Add(touch.fingerId);
          var ignored = _ignoredTouches.Contains(touch.fingerId);
          
          if (touch.phase == TouchPhase.Began && _inputController.IsPointerOverUI(touch.position) && !ignored)
            _ignoredTouches.Add(touch.fingerId);
          
          if(!ignored)
            _touches.Add(touch);
        }

        _ignoredTouches.RemoveAll(touchId => !allTouchesId.Contains(touchId));
      }

      if (PlatformUtilities.IsEditor)
      {
        var position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
          _mousePosition = _inputController.IsPointerOverUI() ? null : position;
        else if (Input.GetMouseButtonUp(0))
          _mousePosition = null;
        else if (_mousePosition.HasValue) 
          _mousePosition = position;
      }
    }

    private void MovePosition()
    {
      if (_touches.Count > 1)
      {
        _canMove = false;
        ClearPositionMove();
      }

      if (_touches.Count == 0 || !_mousePosition.HasValue)
        _canMove = true;

      if (!_canMove)
        return;

      Vector3 position;

      if (_mousePosition.HasValue || _touches.Count == 1)
      {
        var touchPosition = _mousePosition ?? _touches[0].position;
        if (_firstTouchPosition == null)
        {
          _firstTouchPosition = touchPosition;
          _startPosition = _camera.transform.position;
        }

        var pixelDelta = touchPosition - _firstTouchPosition.Value;
        var screenDelta = pixelDelta * PixelToScreenSizeFactor();
        var dragMultiplier = _positionDragMultiplier * (_camera.orthographicSize / 6);
        var fromStartDelta = -new Vector3(screenDelta.x, screenDelta.y, 0) * dragMultiplier;

        _targetPosition = _startPosition + fromStartDelta;
        _targetPosition = ClampPosition(_targetPosition);
        var fromCurrentDelta = _targetPosition - _camera.transform.position;
        _inertiaTargetPosition = _camera.transform.position + fromCurrentDelta * _positionInertiaFactor;
        _inertiaTargetPosition = ClampPosition(_inertiaTargetPosition);
        position = Vector3.Lerp(_camera.transform.position, _targetPosition, _positionLerpFactor * Time.deltaTime);
      }
      else
      {
        _firstTouchPosition = null;
        position = Vector3.Lerp(_camera.transform.position, _inertiaTargetPosition, _positionInertiaLerpFactor * Time.deltaTime);
      }

      _camera.transform.position = position;
    }

    private void Zoom()
    {
      if (_touches.Count == 2)
      {
        var touch0 = _touches[0];
        var touch1 = _touches[1];
        var prevPos0 = touch0.position - touch0.deltaPosition;
        var prevPos1 = touch1.position - touch1.deltaPosition;
        var prevDistance = Vector2.Distance(prevPos0, prevPos1);
        var currentDistance = Vector2.Distance(touch0.position, touch1.position);
        var pixelDelta = currentDistance - prevDistance;
        var screenDelta = pixelDelta * PixelToScreenSizeFactor();
        _targetSize = _camera.orthographicSize - screenDelta * _zoomDragMultiplier; //better use startSize like in movePosition?
      }

      if (PlatformUtilities.IsEditor)
      {
        var delta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(delta) > 0)
          _targetSize = _camera.orthographicSize - delta * _zoomDragMultiplier / 7.5f;
      }

      _targetSize = Mathf.Clamp(_targetSize, 4, 10);
      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetSize, _zoomLerpFactor * Time.deltaTime);
    }

    private float PixelToScreenSizeFactor()
    {
      var dpi = Screen.dpi;
      if (dpi <= 0)
        dpi = 160f;

      if (PlatformUtilities.IsEditor)
        dpi = 400;

      return 2.54f / dpi;
    }

    private Vector3 ClampPosition(Vector3 position)
    {
      position.x = Mathf.Clamp(position.x, -4, 10);
      position.y = Mathf.Clamp(position.y, -4, 10);
      return position;
    }

    private void ClearPositionMove()
    {
      _startPosition = _targetPosition = _inertiaTargetPosition = _camera.transform.position;
      _firstTouchPosition = null;
    }

    private void Clear()
    {
      ClearPositionMove();
      _targetSize = _camera.orthographicSize;
    }
  }
}