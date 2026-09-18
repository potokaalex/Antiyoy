using System.Collections.Generic;
using System.Linq;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitsService : SerializedMonoBehaviour, IInitializable
  {
    [SerializeField] private Dictionary<UnitType, UnitConfig> _unitsConfigs;
    private readonly List<IUnit> _units = new();
    private readonly Dictionary<UnitType, ObjectPool<UnitController>> _pools = new();
    private UnitsAreaCalculator _areaCalculator;
    private GridController _gridController;

    public void Initialize()
    {
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _gridController = Locator.Get<GridController>();
      foreach (var config in _unitsConfigs.Values)
        _pools.Add(config.Type, new(() => Instantiate(config.Prefab, transform),
          x => x.gameObject.SetActive(true), x => x.gameObject.SetActive(false)));
    }

    public void InitialCreateUnits()
    {
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0)), out var redCapitalCell);
      CreateUnit(redCapitalCell, UnitType.Capital, true);

      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 0)), out var blueCapitalCell);
      CreateUnit(blueCapitalCell, UnitType.Capital, true);
    }

    public void Clear()
    {
      for (var i = _units.Count - 1; i >= 0; i--)
        Destroy(_units[i]);
    }

    public void Create(CellController cell, UnitType type, bool hasTurns = true) => CreateUnit(cell, type, hasTurns);

    public void Destroy(IUnit unit)
    {
      if (unit != null)
      {
        var unitController = (UnitController)unit;
        unitController.Dispose();
        _units.Remove(unit);
        _pools[unit.Type].Release(unitController);
      }
    }

    public bool Get(CellController cell, out IUnit unit)
    {
      unit = cell.Unit;
      return unit != null;
    }

    public void GetUnitCreationArea(RegionController region, List<CellController> outResult, UnitType unitType) =>
      _areaCalculator.GetCreationArea(region, outResult, unitType);

    public int GetCost(UnitType type)
    {
      var creationCost = _unitsConfigs[type].CreationCost;
      if (type == UnitType.Farm)
        return creationCost + _units.Count(x => x.Type == UnitType.Farm) * 2;
      return creationCost;
    }

    public bool CanMove(IUnit unit, CellController cell) => CanCreate(cell, unit.Cell.Region.Type, unit.Type) && unit.HasTurns;

    public bool CanCreate(CellController cell, RegionType playerRegion, UnitType unitType)
    {
      if (cell.Region.Type == playerRegion)
      {
        if (cell.HasUnit)
          return JoinOrDefault(unitType, cell.Unit.Type, UnitType.None) != UnitType.None;
        return true;
      }

      return cell.Protection <= GetAttack(unitType);
    }

    public UnitType CalculateJoinedType(UnitType unit, CellController cell, RegionType playerRegion)
    {
      if (cell.Region.Type == playerRegion)
        if (cell.HasUnit)
          return JoinOrDefault(unit, cell.Unit.Type, unit);
      return unit;
    }

    private void CreateUnit(CellController cell, UnitType type, bool hasTurns)
    {
      Destroy(cell.Unit);
      var instance = _pools[type].Get();
      instance.Initialize(_unitsConfigs[type], cell, hasTurns);
      _units.Add(instance);
    }

    private int GetAttack(UnitType type) => _unitsConfigs[type].Attack;

    private UnitType JoinOrDefault(UnitType first, UnitType second, UnitType def)
    {
      if (first.IsWarrior() && second.IsWarrior())
      {
        var joinFactor = GetJoinId(first) + GetJoinId(second);
        foreach (var config in _unitsConfigs.Values)
          if (config.JoinFactor == joinFactor)
            return config.Type;
      }

      return def;
    }

    private int GetJoinId(UnitType unitType) => _unitsConfigs[unitType].JoinFactor;
  }
}