using Client.Configs;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit
{
  public class UnitsService : IInitializable
  {
    private ConfigsProvider _configsProvider;
    private Transform _unitsRoot;
    private ObjectPool<UnitController> _pool;

    public void Initialize()
    {
      _configsProvider = Locator.Get<ConfigsProvider>();
      _unitsRoot = new GameObject("UnitsRoot").transform;
      _pool = new(() => Object.Instantiate(_configsProvider.UnitPrefab, _unitsRoot), x => x.gameObject.SetActive(true),
        x => x.gameObject.SetActive(false));
    }

    public void TryCreate(CellController cell, UnitType type)
    {
      if (!cell.Unit)
        Create(cell, type);
    }

    public void TryDestroy(UnitController unit)
    {
      if (unit)
      {
        unit.Cell.Unit = null;
        _pool.Release(unit);
      }
    }

    public bool TryGet(CellController cell, out UnitController unit)
    {
      unit = cell.Unit;
      return unit;
    }

    private void Create(CellController cell, UnitType type)
    {
      var instance = _pool.Get();
      instance.Initialize(cell, type);
      cell.Unit = instance;
    }
  }
}