using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitController : MonoBehaviour
  {
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private SpriteRenderer _renderer;
    private GridController _gridController;
    private CellController _cell;
    private UnitsService _unitsService;
    private UnitConfig _config;
    private int _turnsCount;

    public CellController Cell => _cell;
    public int Income => _config.Income;
    public UnitType Type => _config.Type;

    public void Initialize(CellController cell, UnitConfig config)
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _cell = cell;
      _config = config;
      _unitsService.TryDestroy(_cell.Unit);
      _cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(_cell.Position);
      RestTurnsCount();
      UpdateDebugText();
      _renderer.sprite = config.Sprite;
    }

    public void GetMoveArea(List<HexCoordinates> outPositions)
    {
      using (ListPool<CellController>.Get(out var cells))
      {
        outPositions.Clear();
        _gridController.GetCellsInRadius(_cell, 1, cells);
        foreach (var cell in cells)
          outPositions.Add(cell.Position);
      }
    }

    public bool Move(CellController cell)
    {
      var friendlyRegion = _cell.Region.Type == cell.Region.Type;

      if (friendlyRegion && cell.Unit)
        return false;

      if (!friendlyRegion)
      {
        _unitsService.TryDestroy(cell.Unit);
        cell.ChangeRegionType(_cell.Region.Type);
      }

      _cell.Unit = null;
      _cell = cell;
      _cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(_cell.Position);
      _turnsCount -= 1;
      UpdateDebugText();
      return true;
    }

    public void ConquerCurrentCell(RegionType type)
    {
      if (_cell.Region.Type != type)
      {
        _cell.ChangeRegionType(type);
        _turnsCount -= 1;
        UpdateDebugText();
      }
    }

    public void RestTurnsCount()
    {
      _turnsCount = _config.TurnsCount;
      UpdateDebugText();
    }

    public bool HasTurns() => _turnsCount > 0;

    private void UpdateDebugText() => _text.SetText($"{_config.Type.ToString()}\n{_turnsCount}");
  }
}