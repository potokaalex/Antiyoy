using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.TilesSelection;
using Client.Unit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Gameplay
{
  public class GameplayController : IInitializable, ITickable
  {
    private readonly List<HexCoordinates> _selectedTiles = new();
    private readonly RegionType _playerRegion = RegionType.Red;
    private CameraController _cameraController;
    private GridController _gridController;
    private RegionController _selectedRegion;
    private UnitsService _unitsService;
    private TilesSelectionView _tilesSelectionView;
    private UnitController _selectedUnit;
    private GameplayUI _gameplayUI;
    private GameplayMode _gameplayMode;
    private int _turnsCount;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
      _gameplayUI = Locator.Get<GameplayUI>();

      _gridController.CreateCells();
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 5)), RegionType.Red);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 4)), RegionType.Red);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(1, 4)), RegionType.Red);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 5)), RegionType.Blue);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 4)), RegionType.Blue);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(7, 4)), RegionType.Blue);

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
          {
            TrySelectRegion(cell);
            TrySelectUnit(cell);
            return;
          }

          if (_gameplayMode == GameplayMode.CreateUnit)
            TryCreateUnit(cell);
          else if (_gameplayMode != GameplayMode.SelectedUnit)
            TrySelectUnit(cell);
          else if (_gameplayMode == GameplayMode.SelectedUnit)
            TryMoveUnit(cell);
        }

        _gameplayMode = GameplayMode.None;
        _tilesSelectionView.ClearView();
        _gameplayUI.ActiveRegionUI(false);
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
      foreach (var unit in _unitsService.Units)
        unit.RestTurnsCount();
      _turnsCount++;
      _gameplayUI.ViewTurnsCount(_turnsCount);
    }

    private void TryMoveUnit(CellController cell)
    {
      if (_selectedTiles.Contains(cell.Position))
        _selectedUnit.Move(cell);
    }

    private void TryCreateUnit(CellController cell)
    {
      if (_selectedTiles.Contains(cell.Position) && _unitsService.TryCreate(cell, UnitType.Peasant, out var unit))
        unit.ConquerCurrentCell(_playerRegion);
    }

    private void TrySelectRegion(CellController cell)
    {
      if (cell.Region.Type == _playerRegion)
      {
        _selectedRegion = cell.Region;
        _gameplayUI.ActiveRegionUI(true);
        _gameplayMode = GameplayMode.SelectedRegion;
      }
    }

    private void TrySelectUnit(CellController cell)
    {
      if (cell.Region.Type == _playerRegion && _unitsService.TryGet(cell, out _selectedUnit) && _selectedUnit.HasTurns())
      {
        _selectedUnit.GetMoveArea(_selectedTiles);
        _tilesSelectionView.ViewTiles(_selectedTiles);
        _gameplayMode = GameplayMode.SelectedUnit;
      }
    }
  }
}