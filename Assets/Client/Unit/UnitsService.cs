using Client.Configs;
using Client.Hex;
using Client.Infrastructure;
using UnityEngine;

namespace Client.Unit
{
  public class UnitsService : IInitializable
  {
    private ConfigsProvider _configsProvider;
    private GridController _gridController;
    private Transform _unitsRoot;

    public void Initialize()
    {
      _configsProvider = Locator.Get<ConfigsProvider>();
      _gridController = Locator.Get<GridController>();
      _unitsRoot = new GameObject("UnitsRoot").transform;
    }

    public void Create(CellController cell, UnitType type)
    {
      var prefab = _configsProvider.UnitsPrefabs[type];
      var instance = Object.Instantiate(prefab, _gridController.HexPositionToWorld(cell.Position), Quaternion.identity, _unitsRoot);
      instance.Initialize(cell);
      cell.Unit = instance;
    }

    public void TryCreate(HexCoordinates point, UnitType type)
    {
      if (_gridController.TryGetCell(point, out var cell) && !cell.Unit) 
        Create(cell, type);
    }

    public void TryDestroy(UnitController unit)
    {
      if (unit) 
        Object.Destroy(unit.gameObject);
    }

    public bool TryGet(HexCoordinates point, out UnitController unit)
    {
      if (_gridController.TryGetCell(point, out var cell))
      {
        unit = cell.Unit;
        return unit;
      }

      unit = null;
      return false;
    }
  }
}