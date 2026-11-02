using Client.MapEditor.Mode;
using Client.MapEditor.UI;
using Client.Menu.MainMenu;
using Client.Project;
using Client.Project.Borders;
using Client.Project.CameraFeatures;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Unit.Code;

namespace Client.MapEditor
{
  public class MapEditorController : IInitializable, ITickable
  {
    private MapsService _mapsService;
    private GridController _gridController;
    private UnitsService _unitsService;
    private RegionsService _regionsService;
    private CameraController _cameraController;
    private BordersService _bordersService;
    private MapEditorUI _mapEditorUI;
    private MainMenuView _mainMenuView;
    private bool _started;
    private IMapEditorModeHandler _currentModeHandler;
    private bool _cellsModeOnlyLand;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _bordersService = Locator.Get<BordersService>();
      _mapsService = Locator.Get<MapsService>();
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _mainMenuView = Locator.Get<MainMenuView>();
    }

    public void Start()
    {
      var map = _mapsService.StartMap;
      _gridController.InitialCreateCells(map);
      _unitsService.InitialCreateUnits(map);
      _regionsService.InitialCreateRegions(map);
      _cameraController.Setup();

      _bordersService.ViewRegionsBorders();
      _mapEditorUI.PlayShow();
      MoveMode();
      _started = true;
    }

    public void Tick()
    {
      if (!_started)
        return;

      if (!_cameraController.GetHitFromMousePoint(out var hit))
        return;

      var hex = _gridController.WorldPositionToHex(hit.point);
      if (!_gridController.IsPositionInGrid(hex))
        return;

      _currentModeHandler?.HandleInput(hex);
    }

    public void MainMenu()
    {
      End();
      _mainMenuView.ShowStart();
    }

    public void MoveMode() => SelectMode(new MapEditorMoveModeHandler());

    public void CreateCellMode(RegionType regionType) => SelectMode(new MapEditorCellsModeHandler(regionType, onlyLand: _cellsModeOnlyLand));

    public void CreateRandomCellMode() => SelectMode(new MapEditorCellsModeHandler(default, random: true, onlyLand: _cellsModeOnlyLand));

    public void DestroyCellMode() => SelectMode(new MapEditorCellsModeHandler(default, destroy: true));

    public void SetCellModeOnlyLand(bool onlyLand)
    {
      _cellsModeOnlyLand = onlyLand;
      if (_currentModeHandler is MapEditorCellsModeHandler cellsModeHandler)
        cellsModeHandler.OnlyLand = onlyLand;
    }

    private void End()
    {
      _started = false;
      _unitsService.Clear();
      _regionsService.Clear();
      _mapEditorUI.Clear();
    }

    private void SelectMode(IMapEditorModeHandler modeHandler)
    {
      _currentModeHandler?.Clear();
      _currentModeHandler = modeHandler;
      _currentModeHandler.Start();
    }
  }
}