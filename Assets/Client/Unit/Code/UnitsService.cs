using System.Collections.Generic;
using System.Linq;
using Client.Configs;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitsService : IInitializable
  {
    private readonly List<IUnitController> _units = new();
    private ConfigsProvider _configsProvider;
    private GridController _gridController;
    private Transform _unitsRoot;
    private ObjectPool<UnitController> _pool;

    public void Initialize()
    {
      _configsProvider = Locator.Get<ConfigsProvider>();
      _gridController = Locator.Get<GridController>();
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

    public void Destroy(IUnitController unit)
    {
      if (unit != null)
      {
        var unitController = (UnitController)unit;
        unitController.Dispose();
        _units.Remove(unit);
        _pool.Release(unitController);
      }
    }

    public bool Get(CellController cell, out IUnitController unit)
    {
      unit = cell.Unit;
      return unit != null;
    }

    public void GetUnitCreationArea(RegionController region, List<CellController> outResult, UnitType unitType)
    {
      using (StackPool<CellController>.Get(out var front))
      {
        outResult.Clear();

        if (unitType == UnitType.Farm)
        {
          GetFarmCreationArea(region, front, outResult);
          return;
        }

        if (unitType == UnitType.Tower)
        {
          outResult.AddRange(region.Cells);
          return;
        }

        foreach (var cell in region.Cells)
        {
          front.Push(cell);
          outResult.Add(cell);
        }

        while (front.Count > 0)
        {
          var cell = front.Pop();
          foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
            if (neighbour.Region.Type != cell.Region.Type && !outResult.Contains(neighbour))
              outResult.Add(neighbour);
        }
      }
    }

    public int GetCost(UnitType type)
    {
      var creationCost = _configsProvider.UnitsConfigs[type].CreationCost;
      if (type == UnitType.Farm)
        return creationCost + _units.Count(x => x.Type == UnitType.Farm) * 2;
      return creationCost;
    }

    private void GetFarmCreationArea(RegionController region, Stack<CellController> front, List<CellController> outResult)
    {
      foreach (var cell in region.Cells)
      {
        if (cell.HasUnit && cell.Unit.Type is UnitType.Capital or UnitType.Farm)
        {
          front.Push(cell);
          outResult.Add(cell);
        }
      }

      while (front.Count > 0)
      {
        var cell = front.Pop();
        foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
          if (neighbour.Region.Type == cell.Region.Type && !outResult.Contains(neighbour))
            outResult.Add(neighbour);
      }
    }

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