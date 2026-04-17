using Client.New.Infrastructure;
using Client.New.Region;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.New.Gameplay
{
  public class GameplayTest : MonoBehaviour
  {
    private CameraController _cameraController;
    private GridController _gridController;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
    }

    private void Update()
    {
      if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        var hit = _cameraController.GetHitFromMousePoint();
        var point = _gridController.WorldPositionToHex(hit.point);
        if (hit && !_gridController.HasCell(point))
        {
          _gridController.CreateCell(point, RegionType.Default);
        }
      }

      if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
      {
        var hit = _cameraController.GetHitFromMousePoint();
        var point = _gridController.WorldPositionToHex(hit.point);
        if (hit && _gridController.TryGetCell(point, out var cell))
        {
          _gridController.DestroyCell(cell);
        }
      }
    }
  }
}