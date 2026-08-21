using System;
using System.Collections.Generic;
using Client.ActionsHistory;
using Client.Borders;
using Client.Gameplay.UI;
using Client.Government;
using Client.Infrastructure;
using Client.Menu.MainMenu;
using Client.Protection;
using Client.Region;
using Client.TilesSelection;
using Client.Unit.Code;
using Client.Unit.Code.Capital;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;

namespace Client.Gameplay
{
  public class GameplayController : IInitializable, ITickable
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
    private GovernmentsService _governmentsService;
    private ProtectionView _protectionView;
    private InputController _inputController;
    private BordersService _bordersService;
    private ActionsHistoryController _actionsHistoryController;
    private CapitalsMarkController _capitalsMarkController;
    private MainMenuView _mainMenuView;
    private GameplayMode _gameplayMode;
    private UnitType _creationUnitType;
    private int _turnsCount;
    private bool _canTick;

    public RegionType CurrentPlayerRegionType { get; private set; } = RegionType.Red;

    public bool CanTick => _canTick;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _regionsService = Locator.Get<RegionsService>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _protectionView = Locator.Get<ProtectionView>();
      _inputController = Locator.Get<InputController>();
      _bordersService = Locator.Get<BordersService>();
      _actionsHistoryController = Locator.Get<ActionsHistoryController>();
      _mainMenuView = Locator.Get<MainMenuView>();
      _capitalsMarkController = Locator.Get<CapitalsMarkController>();
    }

    public void Setup()
    {
      _cameraController.SetActive(true);
      _gridController.InitialCreateCells();
      _unitsService.InitialCreateUnits();
      _regionsService.InitialCreateRegions();
      _bordersService.ViewRegionsBorders();

      _gameplayUI.ViewTurnsCount(_turnsCount);
      _gameplayUI.PlayShow();
      _canTick = true;
    }

    public void Tick()
    {
      if(!_canTick)
        return;

      _cameraController.Tick();
      _capitalsMarkController.Tick();

      if (_inputController.IsClick && !_inputController.IsPointerOverUI())
      {
        if (_cameraController.GetHitFromMousePoint(out var hit) &&
            _gridController.GetCell(_gridController.WorldPositionToHex(hit.point), out var cell))
        {
          if (_gameplayMode == GameplayMode.SelectedRegion)
            ShowBuildingsProtection(cell);

          if (_gameplayMode == GameplayMode.None || _gameplayMode == GameplayMode.SelectedRegion)
            TrySelectRegion(cell, false);

          if (_gameplayMode == GameplayMode.SelectedRegion && cell.Region.Type != CurrentPlayerRegionType)
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

    public void SetCreateUnitMode(UnitType type)
    {
      _gameplayMode = GameplayMode.CreateUnit;
      _creationUnitType = type;
      _unitsService.GetUnitCreationArea(_selectedRegion, _selectedCells, _creationUnitType);
      _tilesSelectionView.ClearView();

      if (type != UnitType.Tower)
        _tilesSelectionView.ViewTiles(_selectedCells);
    }

    public void NextTurn()
    {
      Clear();

      if (CheckWin())
        return;

      if (MoveNextPlayer())
      {
        UpdatePlayerRegions();
        return;
      }

      _turnsCount++;
      _gameplayUI.ViewTurnsCount(_turnsCount);
      CurrentPlayerRegionType = RegionType.Red;
      UpdatePlayerRegions();
      _actionsHistoryController.Clear();
    }

    public void EndGameplay()
    {
      _canTick = false;
      _cameraController.SetActive(false);
    }

    public void Pause()
    {
      Clear();
      _gameplayUI.ShowPause();
    }

    public void UnPause() => _gameplayUI.HidePause();

    public async void MainMenu()
    {
      _mainMenuView.ShowStart();

      await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
      EndGameplay();
    }

    public void SelectLastSelectedRegion()
    {
      Clear();
      TrySelectRegion(_lastSelectedRegion);
    }

    private void UpdatePlayerRegions()
    {
      if (_turnsCount <= 0)
        return;

      foreach (var region in _regionsService.Regions)
        if (region.Type == CurrentPlayerRegionType)
          region.Update();
    }

    private bool MoveNextPlayer()
    {
      var currentIndex = (int)CurrentPlayerRegionType;
      var maxIndex = (int)RegionType.Blue;
      if (currentIndex < maxIndex)
      {
        CurrentPlayerRegionType = (RegionType)(currentIndex + 1);
        return true;
      }

      return false;
    }

    private bool CheckWin()
    {
      using (ListPool<GovernmentController>.Get(out var governments))
      {
        _governmentsService.GetAllAlive(governments);
        if (governments.Count == 1)
        {
          _gameplayUI.ShowEndScreen(governments[0].RegionsType);
          return true;
        }

        return false;
      }
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

    private void TryMoveUnit(CellController cell)
    {
      if (_selectedCells.Contains(cell) && _selectedUnit.CanMove(cell))
      {
        var oldCell = _selectedUnit.Cell;
        var newCellUnitType = cell.Unit?.Type;
        var setRegionTypeResult = SetRegionTypeResult.Create();

        _selectedUnit.Move(cell, ref setRegionTypeResult);
        _actionsHistoryController.MoveUnit(cell, newCellUnitType, oldCell, _selectedUnit.Type, setRegionTypeResult);

        Clear(cell.Region.Type != CurrentPlayerRegionType);
        TrySelectRegion(cell);
        TrySelectUnit(cell);
      }
    }

    private void TryCreateUnit(CellController cell)
    {
      var cost = _unitsService.GetCost(_creationUnitType);
      if (_selectedRegion.Money >= cost)
      {
        if (_selectedCells.Contains(cell) && !(cell.HasUnit && cell.Region.Type == CurrentPlayerRegionType))
        {
          var regionMoney = _selectedRegion.Money;
          var setRegionTypeResult = SetRegionTypeResult.Create();
          var newCellUnitType = cell.Unit?.Type;
          var hasTurns = cell.Region.Type == CurrentPlayerRegionType;

          _regionsService.SetRegionType(cell, CurrentPlayerRegionType, ref setRegionTypeResult);
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

    private void TrySelectRegion(CellController cell, bool forceBordersAnim = true)
    {
      if (cell.Region.Type == CurrentPlayerRegionType && cell.Region.IsAlive && _selectedRegion != cell.Region)
        SelectRegion(cell.Region, forceBordersAnim);
    }

    private void TrySelectRegion(RegionController region, bool forceBordersAnim = true)
    {
      if (region.Type == CurrentPlayerRegionType && region.IsAlive && _selectedRegion != region)
        SelectRegion(region, forceBordersAnim);
    }

    private void TrySelectUnit(CellController cell)
    {
      if (cell.Region.Type == CurrentPlayerRegionType && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.HasTurns)
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
      if (cell.Region.Type == CurrentPlayerRegionType && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.CanViewProtection)
        _protectionView.ViewBuildingsProtection(cell.Region);
    }
  }
}