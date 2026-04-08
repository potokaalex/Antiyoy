using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.New.Gameplay
{
  public class GameplayTest : MonoBehaviour
  {
    private CameraController _cameraController;
    private GridController _gridController;

    public void Initialize(CameraController cameraController, GridController gridController)
    {
      _gridController = gridController;
      _cameraController = cameraController;
    }

    private void Update()
    {
      if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        foreach (var c in _gridController.Cells)
        {
          c.ClearColor();
        }

        var hit = _cameraController.GetHitFromMousePoint();

        if (hit && _gridController.TryGetCell(_gridController.WorldPositionToHex(hit.point), out var cell))
        {
          foreach (var neighbourCell in cell.NeighbourCells)
          {
            neighbourCell.SetColor(Color.blue);
          }
        }
      }
    }
  }
}