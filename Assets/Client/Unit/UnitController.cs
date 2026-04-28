using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit
{
  public class UnitController : MonoBehaviour
  {
    [SerializeField] private TextMeshPro _text;
    private GridController _gridController;
    private CellController _cell;
    private UnitsService _unitsService;
    private UnitType _type;
    private int _turnsCount;

    public CellController Cell => _cell;
    public int MaintenanceCost => 2;

    public void Initialize(CellController cell, UnitType type)
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _cell = cell;
      _type = type;
      _unitsService.TryDestroy(_cell.Unit);
      _cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(_cell.Position);
      RestTurnsCount();
      UpdateDebugText();
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

    public void Move(CellController cell)
    {
      var friendlyRegion = _cell.Region.Type == cell.Region.Type;

      if (friendlyRegion && cell.Unit)
        return;

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
      _turnsCount = 1;
      UpdateDebugText();
    }

    public bool HasTurns() => _turnsCount > 0;

    private void UpdateDebugText() => _text.SetText($"{_type.ToString()}\n{_turnsCount}");
  }
}