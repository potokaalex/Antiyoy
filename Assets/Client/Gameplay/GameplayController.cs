using System.Collections.Generic;
using Client.Gameplay.UI;
using Client.Government;
using Client.Hex;
using Client.Infrastructure;
using Client.Protection;
using Client.Region;
using Client.TilesSelection;
using Client.Unit.Code;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Client.Gameplay
{
  public class GameplayController : IInitializable, ITickable
  {
    private readonly List<CellController> _selectedCells = new();
    private RegionType _currentPlayer = RegionType.Red;
    private CameraController _cameraController;
    private GridController _gridController;
    private RegionController _selectedRegion;
    private UnitsService _unitsService;
    private TilesSelectionView _tilesSelectionView;
    private IUnit _selectedUnit;
    private GameplayUI _gameplayUI;
    private RegionsService _regionsService;
    private GovernmentsService _governmentsService;
    private CapitalsController _capitalsController;
    private ProtectionView _protectionView;
    private GameplayMode _gameplayMode;
    private UnitType _creationUnitType;
    private int _turnsCount;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _regionsService = Locator.Get<RegionsService>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _capitalsController = Locator.Get<CapitalsController>();
      _protectionView = Locator.Get<ProtectionView>();

      _gridController.CreateCells();
      for (var i = 0; i < 4; i++)
        _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(i, 0)), RegionType.Red);
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0)), out var redCapitalCell);
      _capitalsController.SetCapital(redCapitalCell);

      for (var i = 4; i < 9; i++)
        _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(i, 0)), RegionType.Blue);
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 0)), out var blueCapitalCell);
      _capitalsController.SetCapital(blueCapitalCell);

      foreach (var region in _regionsService.Regions)
        region.Money = 100;

      _gameplayUI.ViewTurnsCount(_turnsCount);
    }

    public void Tick()
    {
      if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        if (_cameraController.GetHitFromMousePoint(out var hit) &&
            _gridController.GetCell(_gridController.WorldPositionToHex(hit.point), out var cell))
        {
          if (_gameplayMode == GameplayMode.SelectedRegion)
            ShowBuildingsProtection(cell);

          if (_gameplayMode == GameplayMode.None || _gameplayMode == GameplayMode.SelectedRegion)
            TrySelectRegion(cell);

          if (_gameplayMode == GameplayMode.SelectedRegion && cell.Region.Type != _currentPlayer)
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
      _currentPlayer = RegionType.Red;
      UpdatePlayerRegions();
    }

    public void EndGameplay() => SceneManager.LoadScene(0);

    private void UpdatePlayerRegions()
    {
      if (_turnsCount <= 0)
        return;

      foreach (var region in _regionsService.Regions)
        if (region.Type == _currentPlayer)
          region.Update();
    }

    private bool MoveNextPlayer()
    {
      var currentIndex = (int)_currentPlayer;
      var maxIndex = (int)RegionType.Blue;
      if (currentIndex < maxIndex)
      {
        _currentPlayer = (RegionType)(currentIndex + 1);
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

    private void Clear()
    {
      _gameplayMode = GameplayMode.None;
      _tilesSelectionView.ClearView();
      _gameplayUI.ActiveRegionUI(false);
      _gameplayUI.ClearRegionCreation();
    }

    private void TryMoveUnit(CellController cell)
    {
      if (!_selectedCells.Contains(cell) || _selectedUnit.Move(cell))
      {
        Clear();
        TrySelectRegion(cell);
        TrySelectUnit(cell);
      }
    }

    private void TryCreateUnit(CellController cell)
    {
      var cost = _unitsService.GetCost(_creationUnitType);
      if (_selectedRegion.Money >= cost)
      {
        if (_selectedCells.Contains(cell) && _unitsService.Create(cell, _creationUnitType, _currentPlayer))
        {
          _selectedRegion.Money -= cost;
          TrySelectRegion(cell);
        }
      }

      ReturnToSelectedRegion();
      _gameplayUI.ClearRegionCreation();
    }

    private void ReturnToSelectedRegion()
    {
      var region = _selectedRegion;
      Clear();
      SelectRegion(region);
    }

    private void TrySelectRegion(CellController cell)
    {
      if (cell.Region.Type == _currentPlayer && cell.Region.IsAlive)
        SelectRegion(cell.Region);
    }

    private void TrySelectUnit(CellController cell)
    {
      if (cell.Region.Type == _currentPlayer && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.HasTurns)
      {
        _selectedUnit.GetMoveArea(_selectedCells);
        _tilesSelectionView.ViewTiles(_selectedCells);
        _gameplayMode = GameplayMode.SelectedUnit;
      }
    }

    private void SelectRegion(RegionController region)
    {
      _selectedRegion = region;
      _gameplayUI.ActiveRegionUI(true);
      _gameplayUI.ViewRegionData(_selectedRegion.Money, _selectedRegion.GetIncome());
      _gameplayMode = GameplayMode.SelectedRegion;
    }

    private void ShowBuildingsProtection(CellController cell)
    {
      if (cell.Region.Type == _currentPlayer && _unitsService.Get(cell, out _selectedUnit) && _selectedUnit.CanViewProtection)
        _protectionView.ViewBuildingsProtection(cell.Region);
    }
  }
}