using System.Collections;
using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.CameraFeatures
{
  public class CameraController : MonoBehaviour, ITickable
  {
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraImageController _imageController;
    [SerializeField] private float _positionDragMultiplier;
    [SerializeField] private float _positionLerpFactor;
    [SerializeField] private float _positionInertiaFactor;
    [SerializeField] private float _positionInertiaLerpFactor;
    [SerializeField] private float _zoomDragMultiplier;
    [SerializeField] private float _zoomLerpFactor;
    [SerializeField] private LayerMask _gameplayLayers;
    [SerializeField] private LayerMask _menuLayers;
    private readonly List<Touch> _touches = new();
    private readonly List<int> _ignoredTouches = new();
    private readonly float _positionZ = -10;
    private readonly float _minSize = 4;
    private InputController _inputController;
    private GridController _gridController;
    private Vector2? _mousePosition;
    private Vector2? _firstTouchPosition;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _inertiaTargetPosition;
    private Vector2 _minPosition;
    private Vector2 _maxPosition;
    private bool _canMovePosition;
    private float _targetSize;
    private float _maxSize;
    private Vector2 _center;

    public bool CanMove { get; set; }

    public bool GetHitFromMousePoint(out RaycastHit2D hit)
    {
      var ray = _camera.ScreenPointToRay(Input.mousePosition);
      hit = Physics2D.Raycast(ray.origin, ray.direction);
      return hit;
    }

    public void Tick()
    {
      if (CanMove)
      {
        CalculateTouches();
        MovePosition();
        Zoom();
        _camera.transform.position = ClampPosition(_camera.transform.position);
      }
    }

    public void RenderGameplay() => _camera.cullingMask = _gameplayLayers;

    public void RenderMenu() => _camera.cullingMask = _menuLayers;

    public void SetImageRt(RenderTexture rt) => _imageController.SetImageRt(rt);

    public void ClearImageRt() => _imageController.ClearImageRt();

    public IEnumerator CreateScreenshotCoroutine(RenderTexture rt) => _imageController.CreateScreenshotCoroutine(rt);

    public void Setup()
    {
      var gridMin = (Vector2)_gridController.HexPositionToWorld(HexCoordinates.FromArray2DIndex(Vector2Int.zero));
      var gridMax = (Vector2)_gridController.HexPositionToWorld(HexCoordinates.FromArray2DIndex(_gridController.Size - Vector2Int.one));
      _center = (gridMin + gridMax) / 2f;

      var maxVisibleUnits = (gridMax.y - gridMin.y) * 2f;
      _maxSize = Mathf.Max(maxVisibleUnits / 2f, _minSize);
      SetSize(_maxSize);

      _minPosition = _center - Vector2.one * _maxSize;
      _maxPosition = _center + Vector2.one * _maxSize;
      SetPosition(_center);
    }

    public void Focus(Vector3 position)
    {
      position.z = _positionZ;
      _targetPosition = position;
      _inertiaTargetPosition = position;
    }

    private void Awake()
    {
      _inputController = Locator.Get<InputController>();
      _gridController = Locator.Get<GridController>();
      RenderMenu();
      Clear();
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

          if (!ignored)
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
        _canMovePosition = false;
        ClearPositionMove();
      }

      if (_touches.Count == 0 || !_mousePosition.HasValue)
        _canMovePosition = true;

      if (!_canMovePosition)
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

      _targetSize = Mathf.Clamp(_targetSize, _minSize, _maxSize);
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
      var halfHeight = _camera.orthographicSize;
      var halfWidth = halfHeight * _camera.aspect;
      position.x = Mathf.Clamp(position.x, _minPosition.x + halfWidth, _maxPosition.x - halfWidth);
      position.y = Mathf.Clamp(position.y, _minPosition.y + halfHeight, _maxPosition.y - halfHeight);
      return position;
    }

    private void ClearPositionMove() => SetPosition(_camera.transform.position);

    private void Clear()
    {
      ClearPositionMove();
      SetSize(_camera.orthographicSize);
    }

    private void SetPosition(Vector3 value)
    {
      value.z = _positionZ;
      _startPosition = value;
      _targetPosition = value;
      _inertiaTargetPosition = value; 
      _camera.transform.position = value;
      _firstTouchPosition = null;
    }

    private void SetSize(float value)
    {
      _targetSize = value;
      _camera.orthographicSize = value;
    }
  }
}