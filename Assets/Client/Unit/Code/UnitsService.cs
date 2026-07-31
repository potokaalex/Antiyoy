using System.Collections.Generic;
using System.Linq;
using Client.Configs;
using Client.Hex;
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
    private GridController _gridController;
    private Transform _unitsRoot;
    private ObjectPool<UnitController> _pool;

    public void Initialize()
    {
      _configsProvider = Locator.Get<ConfigsProvider>();
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _gridController = Locator.Get<GridController>();
      _unitsRoot = new GameObject("UnitsRoot").transform;
      _pool = new(() => Object.Instantiate(_configsProvider.UnitPrefab, _unitsRoot), x => x.gameObject.SetActive(true),
        x => x.gameObject.SetActive(false));
    }

    public void InitialCreateUnits()
    {
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0)), out var redCapitalCell);
      CreateUnit(redCapitalCell, UnitType.Capital, true);

      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 0)), out var blueCapitalCell);
      CreateUnit(blueCapitalCell, UnitType.Capital, true);
    }

    public void Create(CellController cell, UnitType type, bool hasTurns = true) => CreateUnit(cell, type, hasTurns);

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

    private void CreateUnit(CellController cell, UnitType type, bool hasTurns)
    {
      Destroy(cell.Unit);
      var instance = _pool.Get();
      instance.Initialize(_configsProvider.UnitsConfigs[type], cell, hasTurns);
      _units.Add(instance);
    }
  }
}