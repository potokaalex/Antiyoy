using System.Collections.Generic;
using Client.ActionsHistory;
using Client.Borders;
using Client.Gameplay.UI;
using Client.Government;
using Client.Infrastructure;
using Client.Protection;
using Client.Region;
using Client.TilesSelection;
using Client.Unit.Code;

namespace Client.Gameplay
{
  public class PlayerController
  {
    private readonly List<CellController> _selectedCells = new();
    private readonly CameraController _cameraController;
    private readonly GridController _gridController;
    private readonly UnitsService _unitsService;
    private readonly TilesSelectionView _tilesSelectionView;
    private readonly GameplayUI _gameplayUI;
    private readonly ProtectionView _protectionView;
    private readonly InputController _inputController;
    private readonly BordersService _bordersService;
    private readonly ActionsHistoryController _actionsHistoryController;
    private readonly GovernmentsService _governmentService;
    private readonly GameFieldController _gameFieldController;
    private readonly RegionsService _regionsService;
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
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _protectionView = Locator.Get<ProtectionView>();
      _inputController = Locator.Get<InputController>();
      _bordersService = Locator.Get<BordersService>();
      _actionsHistoryController = Locator.Get<ActionsHistoryController>();
      _governmentService = Locator.Get<GovernmentsService>();
      _gameFieldController = Locator.Get<GameFieldController>();
      _regionsService = Locator.Get<RegionsService>();
    }

    public void Tick() => UpdatePlayerInput();

    public void SetCreateUnitMode(UnitType type)
    {
      _gameplayMode = GameplayMode.CreateUnit;
      _creationUnitType = type;
      _unitsService.GetUnitCreationArea(_selectedRegion, _selectedCells, _creationUnitType);
      _tilesSelectionView.ClearView();

      if (type != UnitType.Tower)
        _tilesSelectionView.ViewTiles(_selectedCells);
    }

    public void Pause() => Clear();

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
      _tilesSelectionView.ClearView();
      if (clearRegionView)
      {
        _gameplayUI.ActiveRegionUI(false);
        _bordersService.ClearRegionSelectionBorders();
      }

      _gameplayUI.ClearRegionCreation();
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
            TrySelectRegion(cell, false);

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
      if (_gameFieldController.CanMoveUnit(_selectedUnit, cell))
      {
        var oldCell = _selectedUnit.Cell;
        var newCellUnitType = cell.Unit?.Type;
        var setRegionTypeResult = _regionsService.CalculateSetRegionTypeRecoveryData(cell, RegionType);
        _gameFieldController.MoveUnit(_selectedUnit, cell);
        _actionsHistoryController.MoveUnit(cell, newCellUnitType, oldCell, _selectedUnit.Type, setRegionTypeResult);

        Clear(cell.Region.Type != RegionType);
        TrySelectRegion(cell);
        TrySelectUnit(cell);
      }
    }

    private void TryCreateUnit(CellController cell)
    {
      if (_gameFieldController.CanCreateUnit(_creationUnitType, cell, _selectedRegion))
      {
        var regionMoney = _selectedRegion.Money;
        var setRegionTypeResult = _regionsService.CalculateSetRegionTypeRecoveryData(cell, RegionType);
        var newCellUnitType = cell.Unit?.Type;
        _gameFieldController.CreateUnit(_creationUnitType, cell, _selectedRegion);
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

    private void TrySelectRegion(CellController cell, bool forceBordersAnim = true) => SelectRegion(cell.Region, forceBordersAnim);

    private void TrySelectUnit(CellController cell)
    {
      if (cell.Region.Type == RegionType && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.HasTurns)
      {
        _selectedUnit.GetMoveArea(_selectedCells);
        _tilesSelectionView.ViewTiles(_selectedCells);
        _gameplayMode = GameplayMode.SelectedUnit;
      }
    }

    private void SelectRegion(RegionController region, bool forceBordersAnim = true)
    {
      _lastSelectedRegion = region;
      _selectedRegion = region;
      _gameplayUI.ActiveRegionUI(true);
      _gameplayUI.ViewRegionData(_selectedRegion.Money, _selectedRegion.GetIncome());
      _bordersService.ViewRegionSelectionBorders(region, forceBordersAnim);
      _gameplayMode = GameplayMode.SelectedRegion;
    }

    private void ShowBuildingsProtection(CellController cell)
    {
      if (cell.Region.Type == RegionType && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.CanViewProtection)
        _protectionView.ViewBuildingsProtection(cell.Region);
    }
  }
}