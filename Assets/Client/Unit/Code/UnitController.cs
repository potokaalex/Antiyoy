using System.Collections.Generic;
using Client.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitController : MonoBehaviour, IUnit
  {
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private SpriteRenderer _renderer;
    private GridController _gridController;
    private UnitsAreaCalculator _areaCalculator;
    private UnitConfig _config;
    private int _turnsCount;

    public CellController Cell { get; private set; }

    public UnitType Type => _config.Type;

    public bool HasTurns => TurnsCount > 0;

    public int Income => _config.Income;

    public int CapitalReplacementFactor => _config.CapitalReplacementFactor;

    public int Protection => _config.Protection;

    public bool CanViewProtection => Type is UnitType.Capital or UnitType.Tower or UnitType.StrongTower;

    private int TurnsCount
    {
      get => _turnsCount;
      set
      {
        _turnsCount = value;
        if (_config.TurnsCount > 0)
          _text.SetText($"{TurnsCount}");
      }
    }

    public void Initialize(UnitConfig config, CellController cell, bool hasTurns)
    {
      _gridController = Locator.Get<GridController>();
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _config = config;
      SetCell(cell);
      if (hasTurns)
        ResetTurnsCount();
      else
        TurnsCount = 0;
    }

    public void Dispose()
    {
      ClearCell();
      _text.SetText(string.Empty);
    }

    public void ResetTurnsCount() => TurnsCount = _config.TurnsCount;

    public void DecreaseTurnsCount() => TurnsCount = Mathf.Max(0, TurnsCount - 1);

    public void GetMoveArea(List<CellController> outList) => _areaCalculator.GetMoveArea(this, outList);

    public void GetProtectionArea(List<CellController> outList) => _areaCalculator.GetProtectionArea(this, outList, true);

    private void SetCell(CellController cell)
    {
      Cell = cell;
      Cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(Cell.Position);
      SetCellsProtection(true);
    }

    private void ClearCell()
    {
      SetCellsProtection(false);
      Cell.Unit = null;
    }

    private void SetCellsProtection(bool active)
    {
      using (ListPool<CellController>.Get(out var cells))
      {
        _areaCalculator.GetProtectionArea(this, cells, false);
        foreach (var cell in cells)
        {
          if (active)
            cell.AddUnitForProtection(this);
          else
            cell.RemoveUnitForProtection(this);
        }
      }
    }
  }
}