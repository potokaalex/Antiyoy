using System.Collections.Generic;
using Client.Configs;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit
{
  public class UnitsService : IInitializable
  {
    private readonly List<UnitController> _units = new();
    private ConfigsProvider _configsProvider;
    private GridController _gridController;
    private Transform _unitsRoot;
    private ObjectPool<UnitController> _pool;

    public IReadOnlyList<UnitController> Units => _units;

    public void Initialize()
    {
      _configsProvider = Locator.Get<ConfigsProvider>();
      _gridController = Locator.Get<GridController>();
      _unitsRoot = new GameObject("UnitsRoot").transform;
      _pool = new(() => Object.Instantiate(_configsProvider.UnitPrefab, _unitsRoot), x => x.gameObject.SetActive(true),
        x => x.gameObject.SetActive(false));
    }

    public bool TryCreate(CellController cell, UnitType type, RegionType playerRegion, out UnitController unit)
    {
      if (CanCreateUnitAt(cell, playerRegion))
      {
        unit = Create(cell, type);
        return true;
      }

      unit = null;
      return false;
    }

    public void TryDestroy(UnitController unit)
    {
      if (unit)
      {
        unit.Cell.Unit = null;
        _units.Remove(unit);
        _pool.Release(unit);
      }
    }

    public bool TryGet(CellController cell, out UnitController unit)
    {
      unit = cell.Unit;
      return unit;
    }

    public void GetCreateUnitArea(RegionController region, List<HexCoordinates> outResult)
    {
      using (StackPool<CellController>.Get(out var front))
      {
        outResult.Clear();

        foreach (var cell in region.Cells)
        {
          front.Push(cell);
          outResult.Add(cell.Position);
        }

        while (front.Count > 0)
        {
          var cell = front.Pop();
          foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
            if (neighbour.Region.Type != cell.Region.Type && !outResult.Contains(neighbour.Position))
              outResult.Add(neighbour.Position);
        }
      }
    }

    public int GetCost(UnitType type) => _configsProvider.UnitPrefab.Cost;

    private UnitController Create(CellController cell, UnitType type)
    {
      var instance = _pool.Get();
      instance.Initialize(cell, type);
      _units.Add(instance);
      return instance;
    }

    private bool CanCreateUnitAt(CellController cell, RegionType playerRegion)
    {
      var friendlyRegion = cell.Region.Type == playerRegion;

      if (friendlyRegion && cell.Unit)
        return false;

      return true;
    }
  }
}