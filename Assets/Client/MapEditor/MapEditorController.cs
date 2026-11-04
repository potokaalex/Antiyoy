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
using Client.Project.Unit.Code.Capital;

namespace Client.MapEditor
{
  public class MapEditorController : IInitializable, ITickable
  {
    private readonly MapEditorMoveModeHandler _moveModeHandler = new();
    private readonly MapEditorCellsModeHandler _cellsModeHandler = new();
    private readonly MapEditorUnitsModeHandler _unitsModeHandler = new();
    private MapsService _mapsService;
    private GridController _gridController;
    private UnitsService _unitsService;
    private RegionsService _regionsService;
    private CameraController _cameraController;
    private BordersService _bordersService;
    private MapEditorUI _mapEditorUI;
    private MainMenuView _mainMenuView;
    private CapitalsController _capitalsController;
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
      _capitalsController = Locator.Get<CapitalsController>();
    }

    public void Start()
    {
      var map = _mapsService.StartMap;
      _gridController.InitialCreateCells(map);
      _unitsService.InitialCreateUnits(map);
      _regionsService.InitialCreateRegions(map);
      _capitalsController.RemoveDoubleCapitals();
      _cameraController.Setup();

      _bordersService.ViewRegionsBorders();
      _mapEditorUI.PlayShow();
      MoveMode();

      _regionsService.OnBeforeJoinRegions += OnBeforeJoinRegions;
      _regionsService.OnBeforeRegionAddCell += OnBeforeRegionAddCell;
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

    public void MoveMode() => SelectMode(_moveModeHandler);

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

    public void CreateUnitMode(UnitType unitType)
    {
      _unitsModeHandler.Setup(unitType);
      SelectMode(_unitsModeHandler);
    }

    public void DeleteUnitMode()
    {
      _unitsModeHandler.Setup(default, destroy: true);
      SelectMode(_unitsModeHandler);
    }

    private void End()
    {
      _started = false;
      _regionsService.OnBeforeJoinRegions -= OnBeforeJoinRegions;
      _regionsService.OnBeforeRegionAddCell -= OnBeforeRegionAddCell;
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

    private void OnBeforeJoinRegions(RegionController major, RegionController minor)
    {
      if (_capitalsController.HasCapital(major))
        _capitalsController.RemoveCapital(minor);
    }

    private void OnBeforeRegionAddCell(RegionController region, CellController cell)
    {
      if (_capitalsController.HasCapital(region) && cell.HasUnit && cell.Unit.Type == UnitType.Capital) 
        _unitsService.Destroy(cell.Unit);
    }
  }
}