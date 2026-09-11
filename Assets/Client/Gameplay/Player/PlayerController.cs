using Client.ActionsHistory;
using Client.Government;
using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;

namespace Client.Gameplay.Player
{
  public class PlayerController
  {
    private readonly CameraController _cameraController;
    private readonly GridController _gridController;
    private readonly UnitsService _unitsService;
    private readonly RegionsService _regionsService;
    private readonly InputController _inputController;
    private readonly ActionsHistoryController _actionsHistoryController;
    private readonly GovernmentsService _governmentService;
    private readonly GameplayFieldController _gameplayFieldController;
    private readonly PlayerViewController _playerViewController;
    private IUnit _selectedUnit;
    private RegionController _selectedRegion;
    private RegionController _lastSelectedRegion;
    private GameplayMode _gameplayMode;
    private UnitType _creationUnitType;

    public RegionType RegionType { get; }

    public bool Alive => _governmentService.IsAlive(RegionType);

    public PlayerController(RegionType regionType)
    {
      RegionType = regionType;
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _inputController = Locator.Get<InputController>();
      _actionsHistoryController = Locator.Get<ActionsHistoryController>();
      _governmentService = Locator.Get<GovernmentsService>();
      _gameplayFieldController = Locator.Get<GameplayFieldController>();
      _playerViewController = Locator.Get<PlayerViewController>();
    }

    public void Tick() => UpdatePlayerInput();

    public void SetCreateUnitMode(UnitType unitType)
    {
      _gameplayMode = GameplayMode.CreateUnit;
      _creationUnitType = unitType;
      _playerViewController.ViewUnitCreation(_selectedRegion, unitType);
    }

    public void Pause() => Clear();

    public void StartTurn() => _playerViewController.SetPlayerController(this);

    public void EndTurn()
    {
      Clear();
      _actionsHistoryController.Clear();
    }

    public void Undo()
    {
      _actionsHistoryController.Undo();
      SelectLastSelectedRegion();
    }

    private void Clear(bool clearRegionView = true)
    {
      _gameplayMode = GameplayMode.None;
      _selectedRegion = null;
      _playerViewController.Clear(clearRegionView);
    }

    private void SelectLastSelectedRegion()
    {
      Clear();
      TrySelectRegion(_lastSelectedRegion);
    }

    private void UpdatePlayerInput()
    {
      if (_inputController.IsClick && !_inputController.IsPointerOverUI())
      {
        if (_cameraController.GetHitFromMousePoint(out var hit) &&
            _gridController.GetCell(_gridController.WorldPositionToHex(hit.point), out var cell))
        {
          if (_gameplayMode == GameplayMode.SelectedRegion)
            ShowBuildingsProtection(cell);

          if (_gameplayMode == GameplayMode.None || _gameplayMode == GameplayMode.SelectedRegion)
            TrySelectRegion(cell.Region, false);

          if (_gameplayMode == GameplayMode.SelectedRegion && cell.Region.Type != RegionType)
            Clear();
          else if (_gameplayMode == GameplayMode.CreateUnit)
            TryCreateUnit(cell);
          else if (_gameplayMode != GameplayMode.SelectedUnit)
            TrySelectUnit(cell);
          else if (_gameplayMode == GameplayMode.SelectedUnit)
            TryMoveUnit(cell);
        }
        else
          Clear();
      }
    }

    private void TryMoveUnit(CellController cell)
    {
      if (_gameplayFieldController.CanMoveUnit(_selectedUnit, cell))
      {
        var oldCell = _selectedUnit.Cell;
        var newCellUnitType = cell.Unit?.Type;
        var setRegionTypeResult = _regionsService.CalculateSetRegionTypeRecoveryData(cell, RegionType);
        _gameplayFieldController.MoveUnit(_selectedUnit, cell);
        _actionsHistoryController.MoveUnit(cell, newCellUnitType, oldCell, _selectedUnit.Type, setRegionTypeResult);
        Clear(cell.Region.Type != RegionType);
        TrySelectUnit(cell);
      }
    }

    private void TryCreateUnit(CellController cell)
    {
      if (_gameplayFieldController.CanCreateUnit(_creationUnitType, cell, _selectedRegion))
      {
        var regionMoney = _selectedRegion.Money;
        var setRegionTypeResult = _regionsService.CalculateSetRegionTypeRecoveryData(cell, RegionType);
        var newCellUnitType = cell.Unit?.Type;
        _gameplayFieldController.CreateUnit(_creationUnitType, cell, _selectedRegion);
        _actionsHistoryController.CreateUnit(cell, newCellUnitType, regionMoney, setRegionTypeResult);
        Clear(false);
        SelectRegion(cell.Region);
      }
      else
        ReturnToSelectedRegion();
    }

    private void ReturnToSelectedRegion()
    {
      var region = _selectedRegion;
      Clear(false);
      SelectRegion(region);
    }

    private void TrySelectRegion(RegionController region, bool forceBordersAnim = true)
    {
      if (region.Type == RegionType && region.IsAlive && _selectedRegion != region)
        SelectRegion(region, forceBordersAnim);
    }

    private void TrySelectUnit(CellController cell)
    {
      TrySelectRegion(cell.Region);

      if (cell.Region.Type == RegionType && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.HasTurns)
      {
        _gameplayMode = GameplayMode.SelectedUnit;
        _playerViewController.ViewUnitSelection(_selectedUnit);
      }
    }

    private void SelectRegion(RegionController region, bool forceBordersAnim = true)
    {
      _lastSelectedRegion = region;
      _selectedRegion = region;
      _gameplayMode = GameplayMode.SelectedRegion;
      _playerViewController.ViewRegionSelection(_selectedRegion, forceBordersAnim);
    }

    private void ShowBuildingsProtection(CellController cell)
    {
      if (cell.Region.Type == RegionType && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.CanViewProtection)
        _playerViewController.ViewViewBuildingsProtection(cell.Region);
    }
  }
}