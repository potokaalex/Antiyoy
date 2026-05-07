using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using TMPro;
using UnityEngine;

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

    public int CapitalReplacementFactor => _config.CapitalReplacementFactor;

    public void Initialize(CellController cell, UnitConfig config)
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _cell = cell;
      _config = config;
      _cell.Unit = this;
      MovePositionToCell();
      RestTurnsCount();
      UpdateDebugText();
      _renderer.sprite = config.Sprite;
    }

    public void Dispose()
    {
      _cell.Unit = null;
      _text.SetText(string.Empty);
    }

    public void GetMoveArea(List<CellController> outList)
    {
      outList.Clear();
      _gridController.GetCellsInRadius(_cell, 1, outList);
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
      MovePositionToCell();
      _turnsCount--;
      UpdateDebugText();
      return true;
    }

    public void ConquerCurrentCell(RegionType type)
    {
      if (_cell.Region.Type != type)
      {
        _cell.ChangeRegionType(type);
        _turnsCount--;
        UpdateDebugText();
      }
    }

    public void RestTurnsCount()
    {
      _turnsCount = _config.TurnsCount;
      UpdateDebugText();
    }

    public bool HasTurns() => _turnsCount > 0;

    private void UpdateDebugText()
    {
      if (Type == UnitType.Peasant)
        _text.SetText($"{_turnsCount}");
    }

    private void MovePositionToCell() => transform.position = _gridController.HexPositionToWorld(_cell.Position);
  }
}