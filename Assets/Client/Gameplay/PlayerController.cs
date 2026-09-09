using System.Collections.Generic;
using Client.ActionsHistory;
using Client.Borders;
using Client.Gameplay.UI;
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
    private CameraController _cameraController;
    private GridController _gridController;
    private RegionController _selectedRegion;
    private RegionController _lastSelectedRegion;
    private UnitsService _unitsService;
    private TilesSelectionView _tilesSelectionView;
    private IUnit _selectedUnit;
    private GameplayUI _gameplayUI;
    private RegionsService _regionsService;
    private ProtectionView _protectionView;
    private InputController _inputController;
    private BordersService _bordersService;
    private ActionsHistoryController _actionsHistoryController;
    private GameplayMode _gameplayMode;
    private UnitType _creationUnitType;
    private int _turnsCount;
    private bool _canTick;
    private List<PlayerController> _players;
    private PlayerController _currentPlayer;

    public PlayerController(RegionType regionType)
    {
      RegionType = regionType;
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _regionsService = Locator.Get<RegionsService>();
      _protectionView = Locator.Get<ProtectionView>();
      _inputController = Locator.Get<InputController>();
      _bordersService = Locator.Get<BordersService>();
      _actionsHistoryController = Locator.Get<ActionsHistoryController>();
    }

    public RegionType RegionType { get; }

    public void Tick()
    {
      UpdatePlayerInput();
    }

    public void SetCreateUnitMode(UnitType type)//?
    {
      _gameplayMode = GameplayMode.CreateUnit;
      _creationUnitType = type;
      _unitsService.GetUnitCreationArea(_selectedRegion, _selectedCells, _creationUnitType);
      _tilesSelectionView.ClearView();

      if (type != UnitType.Tower)
        _tilesSelectionView.ViewTiles(_selectedCells);
    }

    public void SelectLastSelectedRegion()
    {
      _currentPlayer.Clear();
      TrySelectRegion(_lastSelectedRegion);
    }

    public void Clear(bool clearRegionView = true)
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
      if (_selectedCells.Contains(cell) && _selectedUnit.CanMove(cell))
      {
        var oldCell = _selectedUnit.Cell;
        var newCellUnitType = cell.Unit?.Type;
        var setRegionTypeResult = SetRegionTypeResult.Create();

        _selectedUnit.Move(cell, ref setRegionTypeResult);
        _actionsHistoryController.MoveUnit(cell, newCellUnitType, oldCell, _selectedUnit.Type, setRegionTypeResult);

        Clear(cell.Region.Type != RegionType);
        TrySelectRegion(cell);
        TrySelectUnit(cell);
      }
    }

    private void TryCreateUnit(CellController cell)
    {
      var cost = _unitsService.GetCost(_creationUnitType);
      if (_selectedRegion.Money >= cost)
      {
        if (_selectedCells.Contains(cell) && !(cell.HasUnit && cell.Region.Type == RegionType))
        {
          var regionMoney = _selectedRegion.Money;
          var setRegionTypeResult = SetRegionTypeResult.Create();
          var newCellUnitType = cell.Unit?.Type;
          var hasTurns = cell.Region.Type == RegionType;

          _regionsService.SetRegionType(cell, RegionType, ref setRegionTypeResult);
          _unitsService.Create(cell, _creationUnitType, hasTurns);
          _selectedRegion.Money -= cost;
          _actionsHistoryController.CreateUnit(cell, newCellUnitType, regionMoney, setRegionTypeResult);

          Clear(false);
          SelectRegion(cell.Region);
          return;
        }
      }

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

    private void TrySelectRegion(CellController cell, bool forceBordersAnim = true)
    {
      if (cell.Region.Type == RegionType && cell.Region.IsAlive && _selectedRegion != cell.Region)
        SelectRegion(cell.Region, forceBordersAnim);
    }

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