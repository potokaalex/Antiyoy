using System.Collections.Generic;
using Client.DebugFeatures;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Tile;
using Client.TilesSelection;
using Client.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.Gameplay
{
  public class GameplayController : MonoBehaviour
  {
    [SerializeField] private GameObject _regionPanel;
    [SerializeField] private Button _createUnitButton;
    private CameraController _cameraController;
    private GridController _gridController;
    private GameplayMode _gameplayMode;
    private RegionController _selectedRegion;
    private UnitsService _unitsService;
    private TilesSelectionView _tilesSelectionView;
    private readonly List<HexCoordinates> _selectedTiles = new();
    private UnitController _selectedUnit;
    private readonly RegionType _playerRegion = RegionType.Red;

    private void Start()
    {
      Locator.Get<TilemapController>().FillByBaseTiles();
      _gridController = Locator.Get<GridController>();
      _gridController.CreateCells();
      Locator.Get<DebugController>().CreateCells();

      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 5)), RegionType.Red);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 4)), RegionType.Red);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(1, 4)), RegionType.Red);

      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 5)), RegionType.Blue);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 4)), RegionType.Blue);
      _gridController.ReCreateCell(HexCoordinates.FromArray2DIndex(new Vector2Int(7, 4)), RegionType.Blue);

      _cameraController = Locator.Get<CameraController>();
      _createUnitButton.onClick.AddListener(SetCreateUnitMode);
      _unitsService = Locator.Get<UnitsService>();
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
    }

    public void Update()
    {
      if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
      {
        if (_cameraController.GetHitFromMousePoint(out var hit) &&
            _gridController.TryGetCell(_gridController.WorldPositionToHex(hit.point), out var cell))
        {
          if (_selectedRegion == null)
          {
            if (cell.Region.Type == _playerRegion)
            {
              _selectedRegion = cell.Region;
              _regionPanel.SetActive(true);
              TrySelectUnit(cell);
              return;
            }
          }
          else if (_gameplayMode == GameplayMode.CreateUnit && _selectedTiles.Contains(cell.Position))
          {
            if (_unitsService.TryCreate(cell, UnitType.Peasant, out var unit))
            {
              unit.ConquerCurrentCell(_playerRegion);
            }
          }
          else if (_selectedUnit == null)
          {
            TrySelectUnit(cell);
          }
          else if (_selectedUnit && _selectedTiles.Contains(cell.Position))
          {
            _selectedUnit.Move(cell);
          }
        }

        _tilesSelectionView.ClearView();
        _gameplayMode = GameplayMode.None;
        _selectedRegion = null;
        _regionPanel.SetActive(false);
      }
    }

    private void TrySelectUnit(CellController cell)
    {
      if (_unitsService.TryGet(cell, out _selectedUnit))
      {
        _selectedUnit.GetPositionsInMoveRadius(_selectedTiles);
        _tilesSelectionView.ViewTiles(_selectedTiles);
      }
    }

    private void SetCreateUnitMode()
    {
      _gameplayMode = GameplayMode.CreateUnit;
      _unitsService.GetAreaToCreateUnit(_selectedRegion, _selectedTiles);
      _tilesSelectionView.ViewTiles(_selectedTiles);
    }
  }

  public enum GameplayMode
  {
    None = 0,
    CreateUnit = 1
  }
}