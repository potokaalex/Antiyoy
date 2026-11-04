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
    private readonly MapEditorCellsModeHandler _cellsModeHandler = new();
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

    public void CreateCellMode(RegionType regionType)
    {
      _cellsModeHandler.Setup(regionType);
      SelectMode(_cellsModeHandler);
    }

    public void CreateRandomCellMode()
    {
      _cellsModeHandler.Setup(default, random: true);
      SelectMode(_cellsModeHandler);
    }

    public void DestroyCellMode()
    {
      _cellsModeHandler.Setup(default, destroy: true);
      SelectMode(_cellsModeHandler);
    }

    public void SetCellModeOnlyLand(bool onlyLand) => _cellsModeHandler.OnlyLand = onlyLand;

    public void CreateUnitMode(UnitType unitType) => SelectMode(new MapEditorUnitsModeHandler(unitType));

    public void DeleteUnitMode() => SelectMode(new MapEditorUnitsModeHandler(default, destroy: true));

    private void End()
    {
      _started = false;
      _unitsService.Clear();
      _regionsService.Clear();
      _mapEditorUI.Clear();
      ClearCurrentMode();
    }

    private void SelectMode(IMapEditorModeHandler modeHandler)
    {
      _currentModeHandler?.Clear();
      _currentModeHandler = modeHandler;
      _currentModeHandler.Start();
    }

    private void ClearCurrentMode()
    {
      _currentModeHandler?.Clear();
      _currentModeHandler = null;
    }
  }
}