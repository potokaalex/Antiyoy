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

    public int Protection => _config.Protection;

    public void Initialize(CellController cell, UnitConfig config, RegionType regionType)
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _config = config;
      RestTurnsCount();
      _renderer.sprite = config.Sprite;
      InitialConquer(cell, regionType);
      UpdateDebugText();
    }

    public void Dispose()
    {
      SetCellsProtection(false);
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
      if (!IsFriendlyRegion(cell, _cell.Region.Type) || !cell.Unit)
        return false;

      SetCellsProtection(false);
      _cell.Unit = null;
      Conquer(cell, _cell.Region.Type);
      DecreaseTurnsCount();
      return true;
    }

    public void RestTurnsCount()
    {
      _turnsCount = _config.TurnsCount;
      UpdateDebugText();
    }

    public bool HasTurns() => _turnsCount > 0;

    private void InitialConquer(CellController cell, RegionType regionType)
    {
      if (!IsFriendlyRegion(cell, regionType))
        DecreaseTurnsCount();
      Conquer(cell, regionType);
    }

    private void Conquer(CellController cell, RegionType regionType)
    {
      if (!IsFriendlyRegion(cell, regionType))
      {
        _unitsService.TryDestroy(cell.Unit);
        cell.ChangeRegionType(regionType);
      }

      _cell = cell;
      _cell.Unit = this;
      transform.position = _gridController.HexPositionToWorld(_cell.Position);
      SetCellsProtection(true);
    }

    private bool IsFriendlyRegion(CellController cell, RegionType regionType) => cell.Region.Type == regionType;

    private void UpdateDebugText()
    {
      if (Type == UnitType.Peasant)
        _text.SetText($"{_turnsCount}");
    }

    private void SetCellsProtection(bool active)
    {
      SetCellProtection(active, _cell);
      foreach (var cell in _gridController.GetNeighbourCells(_cell.Position))
        SetCellProtection(active, cell);
    }

    private void SetCellProtection(bool active, CellController cell)
    {
      if (active)
        cell.AddUnitForProtection(this);
      else
        cell.RemoveUnitForProtection(this);
    }

    private void DecreaseTurnsCount()
    {
      _turnsCount = Mathf.Max(0, _turnsCount - 1);
      UpdateDebugText();
    }
  }
}