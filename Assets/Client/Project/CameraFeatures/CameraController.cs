using System.Collections;
using System.Collections.Generic;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Project.CameraFeatures
{
  public class CameraController : MonoBehaviour, ITickable
  {
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraImageController _imageController;
    [SerializeField] private float _positionDragMultiplier; //The smaller the value, the smaller the drag.
    [SerializeField] private float _positionLerpFactor;
    [SerializeField] private float _positionInertiaFactor; //The smaller the value, the smaller the inertia.
    [SerializeField] private float _zoomLerpFactor;
    [SerializeField] private float _zoomTouchSensitivity;
    [SerializeField] private float _zoomWheelSensitivity;
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
    private Vector2 _minPosition;
    private Vector2 _maxPosition;
    private bool _canDrag;
    private float _targetSize;
    private float _maxSize = 4;
    private Vector2 _center;
    private Vector3 _dragInertia;

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
        MoveKinetics();
        Drag();
        Zoom();
        _camera.transform.position = ClampPosition(_camera.transform.position);
      }
    }

    public void RenderGameplay() => _camera.cullingMask = _gameplayLayers;

    public void RenderMenu() => _camera.cullingMask = _menuLayers;

    public void SetImageRt(RenderTexture rt) => _imageController.SetImageRt(rt);

    public void ClearImageRt() => _imageController.ClearImageRt();

    public IEnumerator CreateScreenshotCoroutine(RenderTexture rt) => _imageController.CreateScreenshotCoroutine(rt);

    public void Setup(bool viewFullMap = true)
    {
      var gridMin = (Vector2)_gridController.HexPositionToWorld(HexCoordinates.FromArray2DIndex(Vector2Int.zero));
      var gridMax = (Vector2)_gridController.HexPositionToWorld(HexCoordinates.FromArray2DIndex(_gridController.Size - Vector2Int.one));
      _center = (gridMin + gridMax) / 2f;

      var unitsAbove = 5;
      _minPosition = gridMin - Vector2.one * unitsAbove / 2f;
      _maxPosition = gridMax + Vector2.one * unitsAbove / 2f;
      _maxSize = Mathf.Max((_maxPosition.y - _minPosition.y) / 2f, _minSize);

      if (viewFullMap)
      {
        SetSize(_maxSize);
        SetPosition(_center);
      }
      else
      {
        SetSize(_camera.orthographicSize);
        SetPosition(_camera.transform.position);
      }
    }

    public void Focus(Vector3 position)
    {
      ClearDrag();
      _targetPosition = ClampPosition(position);
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

      if (Input.GetKey(KeyCode.Escape))
        return;

      using (ListPool<int>.Get(out var allTouchesId))
      {
        for (var i = 0; i < Input.touchCount; i++)
        {
          var touch = Input.GetTouch(i);
          allTouchesId.Add(touch.fingerId);
          if (_ignoredTouches.Contains(touch.fingerId))
            continue;

          if (touch.phase == TouchPhase.Began)
          {
            var backGestureEdgeWidth = 40f;
            var isBackGesture = touch.position.x <= backGestureEdgeWidth || touch.position.x >= Screen.width - backGestureEdgeWidth;
            var isOverUI = _inputController.IsPointerOverUI(touch.position);

            if (isBackGesture || isOverUI)
            {
              _ignoredTouches.Add(touch.fingerId);
              continue;
            }
          }

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

        if (_mousePosition.HasValue)
          _touches.Add(new Touch { position = _mousePosition.Value });
      }
    }

    private void Drag()
    {
      if (_touches.Count > 1)
      {
        _canDrag = false;
        ClearDrag();
      }

      if (_touches.Count == 0)
        _canDrag = true;

      if (!_canDrag)
        return;

      if (_touches.Count == 1)
      {
        var touchPosition = _touches[0].position;
        if (_firstTouchPosition == null)
        {
          _firstTouchPosition = touchPosition;
          _startPosition = _camera.transform.position;
        }

        var pixelDelta = touchPosition - _firstTouchPosition.Value;
        var screenDelta = pixelDelta * PixelToScreenSizeFactor();
        var worldDelta = -screenDelta * (_positionDragMultiplier * _camera.orthographicSize);

        var previousTargetPosition = _targetPosition;
        _targetPosition = _startPosition + (Vector3)worldDelta;
        _targetPosition = ClampPosition(_targetPosition);
        _dragInertia = _targetPosition - previousTargetPosition;
      }
      else
        _firstTouchPosition = null;

      _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPosition, _positionLerpFactor * Time.deltaTime);
    }

    private void MoveKinetics()
    {
      if (_touches.Count > 0 || _dragInertia == Vector3.zero)
        return;

      _targetPosition += _dragInertia;
      _dragInertia *= Mathf.Clamp01(Mathf.Pow(_positionInertiaFactor, Time.deltaTime * 60f));
      _targetPosition = ClampPosition(_targetPosition);
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

        if (prevDistance > 0.01f)
        {
          var zoomFactor = prevDistance / currentDistance;
          zoomFactor = Mathf.Pow(zoomFactor, _zoomTouchSensitivity);
          _targetSize *= zoomFactor;
        }
      }

      if (PlatformUtilities.IsEditor)
      {
        var scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
          var zoomFactor = Mathf.Exp(-scroll * _zoomWheelSensitivity);
          _targetSize *= zoomFactor;
        }
      }

      _targetSize = ClampSize(_targetSize);
      var lerpFactor = 1f - Mathf.Exp(-_zoomLerpFactor * Time.deltaTime);
      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetSize, lerpFactor);
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
      var minX = _minPosition.x + halfWidth;
      var maxX = _maxPosition.x - halfWidth;
      var minY = _minPosition.y + halfHeight;
      var maxY = _maxPosition.y - halfHeight;
      position.x = minX > maxX ? (_minPosition.x + _maxPosition.x) / 2f : Mathf.Clamp(position.x, minX, maxX);
      position.y = minY > maxY ? (_minPosition.y + _maxPosition.y) / 2f : Mathf.Clamp(position.y, minY, maxY);
      position.z = _positionZ;
      return position;
    }

    private float ClampSize(float size) => Mathf.Clamp(size, _minSize, _maxSize);

    private void ClearDrag() => SetPosition(_camera.transform.position);

    private void Clear()
    {
      ClearDrag();
      SetSize(_camera.orthographicSize);
    }

    private void SetPosition(Vector3 value)
    {
      value = ClampPosition(value);
      _startPosition = value;
      _targetPosition = value;
      _camera.transform.position = value;
      _firstTouchPosition = null;
      _dragInertia = Vector3.zero;
    }

    private void SetSize(float value)
    {
      value = ClampSize(value);
      _targetSize = value;
      _camera.orthographicSize = value;
    }
  }
}