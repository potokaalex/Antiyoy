using System;
using UnityEngine;

namespace Client
{
  public class CameraController : MonoBehaviour
  {
    [SerializeField] private Camera _camera;
    private Vector3 _positionVelocity;
    private Touch? _firstTouch;
    private Vector3 _startPosition;
    [SerializeField] private float _lerpFactor = 20f;
    [SerializeField] private float _dpiFactor = 1;
    [SerializeField] private float _scaleLerpFactor;
    [SerializeField] private float _scaleDpiFactor;
    private Vector3 _targetPosition;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _smoothTimeOnInput;
    [SerializeField] private float _maxSpeed;
    private Vector3 _currentVelocity;
    private Vector3 _inertiaTargetPosition;
    [SerializeField] private float _inertiaFactor;
    [SerializeField] private float _inertiaLerpFactor;

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
      _inertiaTargetPosition = _startPosition = _targetPosition = _camera.transform.position;
    }

    private void Update()
    {
      //float cm = pixels / dpi * 2.54f;

/*
#if UNITY_EDITOR
      Move();
      Scroll();
#else
      */
      MobileMove();
      //MobileScroll();
//#endif
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

    private void MobileMove()
    {
      MobileMove1();
      return;

      //scroll
      if (Input.touchCount == 2)
      {
        var touch0 = Input.GetTouch(0);
        var touch1 = Input.GetTouch(1);
        var prevPos0 = touch0.position - touch0.deltaPosition;
        var prevPos1 = touch1.position - touch1.deltaPosition;
        var prevDistance = Vector2.Distance(prevPos0, prevPos1);
        var currentDistance = Vector2.Distance(touch0.position, touch1.position);
        var pixelDelta = currentDistance - prevDistance;
        var screenDelta = pixelDelta * PixelToScreenSizeFactor();
        var targetSize = _camera.orthographicSize - screenDelta * _scaleDpiFactor;
        var size = Mathf.Lerp(_camera.orthographicSize, targetSize, _scaleLerpFactor * Time.deltaTime);

        _camera.orthographicSize = Mathf.Clamp(size, 2, 8);
      }
    }

    private void MobileMove1()
    {
      Vector3 position;

      if (Input.touchCount == 1)
      {
        var touch = Input.GetTouch(0);
        if (_firstTouch == null)
        {
          _firstTouch = touch;
          _startPosition = _camera.transform.position;
        }

        var pixelDelta = touch.position - _firstTouch.Value.position;
        var screenDelta = pixelDelta * PixelToScreenSizeFactor();
        var fromStartDelta = -new Vector3(screenDelta.x, screenDelta.y, 0) * _dpiFactor;
        _targetPosition = _startPosition + fromStartDelta;
        var fromCurrentDelta = _targetPosition - _camera.transform.position;
        _inertiaTargetPosition = _camera.transform.position + fromCurrentDelta * _inertiaFactor;
        position = Vector3.Lerp(_camera.transform.position, _targetPosition, _lerpFactor * Time.deltaTime);
      }
      else
      {
        if (_firstTouch.HasValue)
          _firstTouch = null;

        position = Vector3.Lerp(_camera.transform.position, _inertiaTargetPosition, _inertiaLerpFactor * Time.deltaTime);
      }

      position.x = Mathf.Clamp(position.x, -4, 10);
      position.y = Mathf.Clamp(position.y, -4, 10);
      _camera.transform.position = position;
    }

    private float PixelToScreenSizeFactor()
    {
      var dpi = Screen.dpi;
      if (dpi <= 0)
        dpi = 160f;

      return 2.54f / dpi;
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
  }
}