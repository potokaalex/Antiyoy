using System.Collections.Generic;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitController : MonoBehaviour, IUnit
  {
    [SerializeField] private SpriteRenderer _renderer;
    private GridController _gridController;
    private UnitsAreaCalculator _areaCalculator;
    private UnitConfig _config;

    public CellController Cell { get; private set; }

    public UnitType Type => _config.Type;

    public bool HasTurns { get; private set; }

    public int Income => _config.Income;

    public int CapitalReplacementFactor => _config.CapitalReplacementFactor;

    public int Protection => _config.Protection;

    public bool CanViewProtection => Type is UnitType.Capital or UnitType.Tower or UnitType.StrongTower;

    public Vector3 Position { set => transform.position = value; }

    protected SpriteRenderer Renderer => _renderer;

    public void Setup(UnitConfig config, CellController cell, bool hasTurns)
    {
      _gridController = Locator.Get<GridController>();
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _config = config;
      SetCell(cell);
      if (hasTurns)
        ResetTurns();
      else
        HasTurns = false;
      SetupSprite();
    }

    public void Clear() => ClearCell();

    public void ResetTurns() => HasTurns = _config.HasTurns;

    public void GetMoveArea(List<CellController> outList) => _areaCalculator.GetMoveArea(this, outList);

    public void GetProtectionArea(List<CellController> outList) => _areaCalculator.GetProtectionArea(this, outList, true);

    protected virtual void SetupSprite() => _renderer.sprite = _config.Sprite;

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