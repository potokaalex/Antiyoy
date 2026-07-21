using Client.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace Client
{
  public class EventSystemController : IInitializable
  {
    private PointerEventData _eventData;
    private EventSystem _eventSystem;

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
  }
}