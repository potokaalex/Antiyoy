using System.Linq;
using Client.New.Government;
using Client.New.Infrastructure;
using Client.New.Region;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace Client.New.Gameplay
{
  public class GameplayTest : MonoBehaviour
  {
    private CameraController _cameraController;
    private GridController _gridController;
    private GovernmentsService _governmentsService;
    private RegionType? _currentRegionType;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _governmentsService = Locator.Get<GovernmentsService>();
    }

    private void OnGUI()
    {
      var width = 100;
      var height = 50;
      var space = height + 25;

      var labelStyle = new GUIStyle(GUI.skin.label);
      labelStyle.fontSize = 18;

      GUI.Label(new Rect(0, 0, width * 2, height), $"Regions: {_currentRegionType}", labelStyle);

      if (GUI.Button(new Rect(0, space, width, height), "None"))
        _currentRegionType = null;
      if (GUI.Button(new Rect(0, space * 2, width, height), "Default"))
        _currentRegionType = RegionType.Default;
      if (GUI.Button(new Rect(0, space * 3, width, height), "Red"))
        _currentRegionType = RegionType.Red;
      if (GUI.Button(new Rect(0, space * 4, width, height), "Blue"))
        _currentRegionType = RegionType.Blue;

      using var d = ListPool<GovernmentController>.Get(out var governments);
      _governmentsService.GetAll(governments);
      var govLog = string.Empty;
      foreach (var government in governments)
      {
        govLog +=
          $"Type: {government.Regions[0].Type}, RegionsCount: {government.Regions.Count}, CellsCount: {government.Regions.Sum(x => x.Cells.Count)}\n";
      }

      GUI.Label(new Rect(Screen.width - 500, 0, 500, 1000), govLog, labelStyle);
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