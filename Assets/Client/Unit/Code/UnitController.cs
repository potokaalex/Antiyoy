using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
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
    private UnitsService _unitsService;
    private UnitsAreaCalculator _areaCalculator;
    private UnitConfig _config;
    private int _turnsCount;

    public CellController Cell { get; private set; }

    public UnitType Type => _config.Type;

    public bool HasTurns => TurnsCount > 0;

    public int Income => _config.Income;

    public int CapitalReplacementFactor => _config.CapitalReplacementFactor;

    public int Protection => _config.Protection;

    public bool CanViewProtection => Type is UnitType.Capital or UnitType.Tower;

    private int TurnsCount
    {
      get => _turnsCount;
      set
      {
        _turnsCount = value;
        UpdateDebugText();
      }
    }

    public void Initialize(UnitConfig config)
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _config = config;
      ResetTurnsCount();
      _renderer.sprite = config.Sprite;
    }

    public void InitialConquer(CellController cell, RegionType regionType)
    {
      if (!IsFriendlyRegion(cell, regionType))
        DecreaseTurnsCount();
      Conquer(cell, regionType);
    }

    public void Dispose()
    {
      SetCellsProtection(false);
      Cell.Unit = null;
      _text.SetText(string.Empty);
    }

    public void ResetTurnsCount() => TurnsCount = _config.TurnsCount;

    public void GetMoveArea(List<CellController> outList) => _areaCalculator.GetMoveArea(this, outList);

    public bool Move(CellController cell)
    {
      if (IsFriendlyRegion(cell, Cell.Region.Type) && cell.HasUnit)
        return false;

      SetCellsProtection(false);
      Cell.Unit = null;
      Conquer(cell, Cell.Region.Type);
      DecreaseTurnsCount();
      return true;
    }

    public void GetProtectionArea(List<CellController> outList) => _areaCalculator.GetProtectionArea(this, outList, true);

    private void Conquer(CellController cell, RegionType regionType)
    {
      if (!IsFriendlyRegion(cell, regionType))
      {
        _unitsService.Destroy(cell.Unit);
        cell.ChangeRegionType(regionType);
      }

      Cell = cell;
      Cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(Cell.Position);
      SetCellsProtection(true);
    }

    private bool IsFriendlyRegion(CellController cell, RegionType regionType) => cell.Region.Type == regionType;

    private void UpdateDebugText()
    {
      if (Type == UnitType.Peasant)
        _text.SetText($"{TurnsCount}");
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

    private void DecreaseTurnsCount() => TurnsCount = Mathf.Max(0, TurnsCount - 1);
  }
}