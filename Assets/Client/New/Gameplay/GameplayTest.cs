using Client.New.Region;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Client.New.Gameplay
{
  public class GameplayTest : MonoBehaviour
  {
    private CameraController _cameraController;
    private GridController _gridController;
    private GameController _gameController;

    [Inject]
    private void Construct(CameraController cameraController, GridController gridController, GameController gameController)
    {
      _gameController = gameController;
      _gridController = gridController;
      _cameraController = cameraController;
    }

    private void Update()
    {
      if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        var hit = _cameraController.GetHitFromMousePoint();
        var point = _gridController.WorldPositionToHex(hit.point);
        if (hit && !_gridController.HasCell(point))
        {
          _gameController.CreateCell(point, RegionType.Default);
        }
      }

      if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
      {
        var hit = _cameraController.GetHitFromMousePoint();
        var point = _gridController.WorldPositionToHex(hit.point);
        if (hit && _gridController.TryGetCell(point, out var cell))
        {
          _gameController.DestroyCell(cell);
        }
      }
    }
  }
}