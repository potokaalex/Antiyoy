using System;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace Client
{
  public class InputController : IInitializable, ITickable
  {
    private PointerEventData _eventData;
    private EventSystem _eventSystem;
    private DateTime _startTime;
    private Vector3 _startPosition;

    public bool IsClick { get; private set; }

    public void Initialize()
    {
      _eventSystem = EventSystem.current;
      _eventData = new PointerEventData(_eventSystem);
    }

    public bool IsPointerOverUI()
    {
      using (ListPool<RaycastResult>.Get(out var results))
      {
        _eventData ??= new PointerEventData(_eventSystem);
        _eventData.position = Input.mousePosition;
        _eventSystem.RaycastAll(_eventData, results);
        return results.Count > 0;
      }
    }

    public void Tick()
    {
      IsClick = false;

      if (Input.GetMouseButtonDown(0))
      {
        _startTime = DateTime.UtcNow;
        _startPosition = Input.mousePosition;
      }

      if (Input.GetMouseButtonUp(0))
        if ((DateTime.UtcNow - _startTime).TotalSeconds < 0.3f &&
            Vector2.Distance(_startPosition, Input.mousePosition) < _eventSystem.pixelDragThreshold)
          IsClick = true;
    }
  }
}