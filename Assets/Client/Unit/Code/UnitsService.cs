using System;
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
    private readonly Dictionary<UnitType, ObjectPool<UnitController>> _pools = new();
    private UnitsAreaCalculator _areaCalculator;
    private GridController _gridController;

    public event Action<IUnit> OnCreate;

    public event Action<IUnit> OnDestroy;

    public List<IUnit> Units { get; } = new();

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
      CreateUnit(redCapitalCell, UnitType.Capital, false);

      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 0)), out var blueCapitalCell);
      CreateUnit(blueCapitalCell, UnitType.Capital, false);

      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 8)), out var pine1);
      CreateUnit(pine1, UnitType.Pine, false);
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(1, 8)), out var pine2);
      CreateUnit(pine2, UnitType.Pine, false);

      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(7, 8)), out var palm1);
      CreateUnit(palm1, UnitType.Palm, false);
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 8)), out var palm2);
      CreateUnit(palm2, UnitType.Palm, false);
    }

    public void Clear()
    {
      for (var i = Units.Count - 1; i >= 0; i--)
        Destroy(Units[i]);
    }

    public void Create(CellController cell, UnitType type, bool hasTurns = true) => CreateUnit(cell, type, hasTurns);

    public void Destroy(IUnit unit)
    {
      if (unit != null)
      {
        OnDestroy?.Invoke(unit);
        var unitController = (UnitController)unit;
        unitController.Clear();
        Units.Remove(unit);
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
        return creationCost + Units.Count(x => x.Type == UnitType.Farm) * 2;
      return creationCost;
    }

    public bool CanMove(IUnit unit, CellController cell) => CanCreate(cell, unit.Cell.Region.Type, unit.Type) && unit.HasTurns;

    public bool CanCreate(CellController cell, RegionType playerRegion, UnitType unitType)
    {
      if (cell.Region.Type == playerRegion)
      {
        if (cell.HasUnit)
          return JoinOrDefault(cell.Unit.Type, unitType, UnitType.None) != UnitType.None;
        return true;
      }

      return cell.Protection <= GetAttack(unitType);
    }

    public UnitType CalculateJoinedType(UnitType unitType, CellController cell, RegionType playerRegion)
    {
      if (cell.Region.Type == playerRegion)
        if (cell.HasUnit)
          return JoinOrDefault(cell.Unit.Type, unitType, unitType);
      return unitType;
    }

    public Sprite GetSprite(UnitType unitType) => _unitsConfigs[unitType].Sprite;

    private void CreateUnit(CellController cell, UnitType type, bool hasTurns)
    {
      Destroy(cell.Unit);
      var unit = _pools[type].Get();
      unit.Setup(_unitsConfigs[type], cell, hasTurns);
      Units.Add(unit);
      OnCreate?.Invoke(unit);
    }

    private int GetAttack(UnitType type) => _unitsConfigs[type].Attack;

    private UnitType JoinOrDefault(UnitType current, UnitType next, UnitType def)
    {
      if ((current.IsTree() || current == UnitType.Grave) && next.IsWarrior())
        return next;

      if (current == UnitType.Tower && next == UnitType.StrongTower)
        return UnitType.StrongTower;

      if (current.IsWarrior() && next.IsWarrior())
      {
        var joinFactor = GetJoinId(current) + GetJoinId(next);
        foreach (var config in _unitsConfigs.Values)
          if (config.JoinFactor == joinFactor)
            return config.Type;
      }

      return def;
    }

    private int GetJoinId(UnitType unitType) => _unitsConfigs[unitType].JoinFactor;
  }
}