using System.Collections.Generic;
using System.Linq;
using Client.Configs;
using Client.Infrastructure;
using Client.Region;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitsService : IInitializable
  {
    private readonly List<IUnit> _units = new();
    private ConfigsProvider _configsProvider;
    private UnitsAreaCalculator _areaCalculator;
    private Transform _unitsRoot;
    private ObjectPool<UnitController> _pool;

    public void Initialize()
    {
      _configsProvider = Locator.Get<ConfigsProvider>();
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _unitsRoot = new GameObject("UnitsRoot").transform;
      _pool = new(() => Object.Instantiate(_configsProvider.UnitPrefab, _unitsRoot), x => x.gameObject.SetActive(true),
        x => x.gameObject.SetActive(false));
    }

    public bool Create(CellController cell, UnitType type, RegionType regionType)
    {
      if (CanCreateUnitAt(cell, regionType))
      {
        CreateUnit(cell, type, regionType);
        return true;
      }

      return false;
    }

    public void Destroy(IUnit unit)
    {
      if (unit != null)
      {
        var unitController = (UnitController)unit;
        unitController.Dispose();
        _units.Remove(unit);
        _pool.Release(unitController);
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
      var creationCost = _configsProvider.UnitsConfigs[type].CreationCost;
      if (type == UnitType.Farm)
        return creationCost + _units.Count(x => x.Type == UnitType.Farm) * 2;
      return creationCost;
    }

    public Sprite GetSprite(UnitType unitType) => _configsProvider.UnitsConfigs[unitType].Sprite;

    private void CreateUnit(CellController cell, UnitType type, RegionType regionType)
    {
      Destroy(cell.Unit);
      var instance = _pool.Get();
      instance.Initialize(cell, _configsProvider.UnitsConfigs[type], regionType);
      _units.Add(instance);
    }

    private bool CanCreateUnitAt(CellController cell, RegionType playerRegion)
    {
      var friendlyRegion = cell.Region.Type == playerRegion;

      if (friendlyRegion && cell.HasUnit)
        return false;

      return true;
    }
  }
}