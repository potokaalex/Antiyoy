using System.Collections.Generic;
using Client.Government;
using Client.Hex;
using Client.Infrastructure;
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
    private UnitController _selectedUnit;
    private GameplayUI _gameplayUI;
    private RegionsService _regionsService;
    private GovernmentsService _governmentsService;
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

      _gridController.CreateCells();
      for (var i = 0; i < 4; i++)
        _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(i, 0)), RegionType.Red);
      _gridController.TryGetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0)), out var redCapitalCell);
      _unitsService.TryCreate(redCapitalCell, UnitType.Capital, RegionType.Red, out _);

      for (var i = 4; i < 9; i++)
        _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(i, 0)), RegionType.Blue);
      _gridController.TryGetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 0)), out var blueCapitalCell);
      _unitsService.TryCreate(blueCapitalCell, UnitType.Capital, RegionType.Blue, out _);

      foreach (var region in _regionsService.Regions)
        region.Money = 100;

      _gameplayUI.ViewTurnsCount(_turnsCount);
    }

    public void Tick()
    {
      if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        if (_cameraController.GetHitFromMousePoint(out var hit) &&
            _gridController.TryGetCell(_gridController.WorldPositionToHex(hit.point), out var cell))
        {
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
      _unitsService.GetCreateUnitArea(_selectedRegion, _selectedCells, _creationUnitType);
      _tilesSelectionView.ViewTiles(_selectedCells);
      _gameplayUI.ViewUnitPrice(_unitsService.GetCost(type));
    }

    public void NextTurn()
    {
      Clear();

      if (CheckWin())
        return;

      if (MoveNextPlayer())
        return;

      _currentPlayer = RegionType.Red;
      foreach (var unit in _unitsService.Units)
        unit.RestTurnsCount();
      foreach (var region in _regionsService.Regions)
        region.OnNextTurn();

      _turnsCount++;
      _gameplayUI.ViewTurnsCount(_turnsCount);
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

    public void EndGameplay()
    {
      SceneManager.LoadScene(0);
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
      _gameplayUI.ViewUnitPrice(0);
    }

    private void TryMoveUnit(CellController cell)
    {
      if (_selectedCells.Contains(cell))
      {
        if (_selectedUnit.Move(cell))
        {
          Clear();
          TrySelectRegion(cell);
        }
      }
      else
      {
        Clear();
        TrySelectRegion(cell);
      }
    }

    private void TryCreateUnit(CellController cell)
    {
      var cost = _unitsService.GetCost(_creationUnitType);
      if (_selectedRegion.Money >= cost)
      {
        if (_selectedCells.Contains(cell) && _unitsService.TryCreate(cell, _creationUnitType, _currentPlayer, out var unit))
        {
          unit.ConquerCurrentCell(_currentPlayer);
          _selectedRegion.Money -= cost;
          _gameplayUI.ViewRegionData(_selectedRegion.Money, _selectedRegion.GetIncome());
        }
      }

      ReturnToSelectedRegion();
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
      {
        SelectRegion(cell.Region);
        TrySelectUnit(cell);
      }
    }

    private void TrySelectUnit(CellController cell)
    {
      if (cell.Region.Type == _currentPlayer && _unitsService.TryGet(cell, out _selectedUnit) && _selectedUnit.HasTurns())
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
  }
}