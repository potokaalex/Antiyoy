using System.Collections.Generic;
using Client.Government;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.TilesSelection;
using Client.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Client.Gameplay
{
  public class GameplayController : IInitializable, ITickable
  {
    private readonly List<HexCoordinates> _selectedTiles = new();
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

      for (var i = 4; i < 9; i++)
        _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(i, 0)), RegionType.Blue);

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
          if (_gameplayMode == GameplayMode.None)
            TrySelectRegion(cell);
          else if (_gameplayMode == GameplayMode.SelectedRegion && cell.Region.Type != _currentPlayer)
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

    public void SetCreateUnitMode()
    {
      _gameplayMode = GameplayMode.CreateUnit;
      _unitsService.GetCreateUnitArea(_selectedRegion, _selectedTiles);
      _tilesSelectionView.ViewTiles(_selectedTiles);
    }

    public void NexTurn()
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
        region.ApplyIncome();

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
    }

    private void TryMoveUnit(CellController cell)
    {
      if (_selectedTiles.Contains(cell.Position))
      {
        if (_selectedUnit.Move(cell))
        {
          _tilesSelectionView.ClearView();
          _gameplayMode = GameplayMode.SelectedRegion;
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
      var unitType = UnitType.Peasant;
      if (_selectedRegion.SpendMoney(_unitsService.GetCost(unitType)))
      {
        if (_selectedTiles.Contains(cell.Position) && _unitsService.TryCreate(cell, UnitType.Peasant, out var unit))
        {
          unit.ConquerCurrentCell(_currentPlayer);
          _gameplayUI.ViewRegionData(_selectedRegion.Money, _selectedRegion.GetIncome());
        }
      }

      _tilesSelectionView.ClearView();
      _gameplayMode = GameplayMode.SelectedRegion;
    }

    private void TrySelectRegion(CellController cell)
    {
      if (cell.Region.Type == _currentPlayer)
      {
        _selectedRegion = cell.Region;
        _gameplayUI.ActiveRegionUI(true);
        _gameplayUI.ViewRegionData(_selectedRegion.Money, _selectedRegion.GetIncome());
        _gameplayMode = GameplayMode.SelectedRegion;
      }

      TrySelectUnit(cell);
    }

    private void TrySelectUnit(CellController cell)
    {
      if (cell.Region.Type == _currentPlayer && _unitsService.TryGet(cell, out _selectedUnit) && _selectedUnit.HasTurns())
      {
        _selectedUnit.GetMoveArea(_selectedTiles);
        _tilesSelectionView.ViewTiles(_selectedTiles);
        _gameplayMode = GameplayMode.SelectedUnit;
      }
    }
  }
}