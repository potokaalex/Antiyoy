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
    private RegionType? _currentRegionType;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
    }

    private void OnGUI()
    {
      var width = 100;
      var height = 50;
      var space = height + 25;

      GUI.Label(new Rect(0, 0, width, height), $"Regions: {_currentRegionType}");

      if (GUI.Button(new Rect(0, space, width, height), "None"))
        _currentRegionType = null;
      if (GUI.Button(new Rect(0, space * 2, width, height), "Default"))
        _currentRegionType = RegionType.Default;
      if (GUI.Button(new Rect(0, space * 3, width, height), "Red"))
        _currentRegionType = RegionType.Red;
      if (GUI.Button(new Rect(0, space * 4, width, height), "Blue"))
        _currentRegionType = RegionType.Blue;
    }

    private void Update()
    {
      if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        var hit = _cameraController.GetHitFromMousePoint();
        if (hit)
        {
          var point = _gridController.WorldPositionToHex(hit.point);

          if (_gridController.TryGetCell(point, out var cell))
            _gridController.DestroyCell(cell);

          if (_currentRegionType.HasValue)
            _gridController.CreateCell(point, _currentRegionType.Value);
        }
      }
    }
  }
}