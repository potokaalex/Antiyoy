using System.Linq;
using Client.Government;
using Client.Infrastructure;
using Client.Region;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace Client.Gameplay
{
  public enum MapEditorType
  {
    None = 0,
    DestroyCell = 1,
    CreateNeutral = 2,
    CreateRed = 3,
    CreateBlue = 4
  }

  public class GameplayTest : MonoBehaviour
  {
    private CameraController _cameraController;
    private GridController _gridController;
    private GovernmentsService _governmentsService;
    private MapEditorType _mapEditorType;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _governmentsService = Locator.Get<GovernmentsService>();
    }

    private void OnGUI()
    {
      var labelStyle = new GUIStyle(GUI.skin.label);
      labelStyle.fontSize = 18;

      ViewMapEditor(labelStyle);
      ViewGovernmentDebug(labelStyle);
    }

    private void Update()
    {
      UpdateMapEditor();
    }

    private void UpdateMapEditor()
    {
      if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        var hit = _cameraController.GetHitFromMousePoint();
        if (hit)
        {
          var point = _gridController.WorldPositionToHex(hit.point);

          if (_mapEditorType == MapEditorType.DestroyCell)
            _gridController.TryDestroyCell(point);
          else if (_mapEditorType == MapEditorType.CreateNeutral)
            _gridController.ReCreateCell(point, RegionType.Neutral);
          else if (_mapEditorType == MapEditorType.CreateRed)
            _gridController.ReCreateCell(point, RegionType.Red);
          else if (_mapEditorType == MapEditorType.CreateBlue)
            _gridController.ReCreateCell(point, RegionType.Blue);
        }
      }
    }

    private void ViewMapEditor(GUIStyle labelStyle)
    {
      var width = 100;
      var height = 50;
      var space = height + 25;

      GUI.Label(new Rect(0, 0, width * 5, height), $"MapEditor: {_mapEditorType}", labelStyle);

      if (GUI.Button(new Rect(0, space, width, height), "None"))
        _mapEditorType = MapEditorType.None;
      if (GUI.Button(new Rect(0, space * 2, width, height), "Destroy"))
        _mapEditorType = MapEditorType.DestroyCell;
      if (GUI.Button(new Rect(0, space * 3, width, height), "Neutral"))
        _mapEditorType = MapEditorType.CreateNeutral;
      if (GUI.Button(new Rect(0, space * 4, width, height), "Red"))
        _mapEditorType = MapEditorType.CreateRed;
      if (GUI.Button(new Rect(0, space * 5, width, height), "Blue"))
        _mapEditorType = MapEditorType.CreateBlue;
    }

    private void ViewGovernmentDebug(GUIStyle labelStyle)
    {
      using var d = ListPool<GovernmentController>.Get(out var governments);
      _governmentsService.GetAll(governments);
      var govLog = string.Empty;
      foreach (var government in governments)
      {
        govLog +=
          $"Type: {government.RegionsType}, RegionsCount: {government.Regions.Count}, CellsCount: {government.Regions.Sum(x => x.Cells.Count)}\n";
      }

      GUI.Label(new Rect(Screen.width - 500, 0, 500, 1000), govLog, labelStyle);
    }
  }
}