using System;
using System.Collections.Generic;
using Client.Project.Government;
using Client.Project.Infrastructure;
using UnityEngine.Pool;

namespace Client.Project.Region
{
  public class RegionsFactory : IInitializable
  {
    private readonly List<RegionController> _regions = new();
    private GovernmentsService _governmentsService;
    private ObjectPool<RegionController> _pool;

    public IReadOnlyList<RegionController> ActiveRegions => _regions;

    public void Initialize()
    {
      _governmentsService = Locator.Get<GovernmentsService>();
      _pool = new ObjectPool<RegionController>(() => new RegionController());
    }

    public void Create(CellController cell, Action<RegionController, CellController> addFunc, RegionType type = RegionType.Neutral, int money = 0)
    {
      using (ListPool<CellController>.Get(out var list))
      {
        list.Add(cell);
        Create(list, addFunc, type, money);
      }
    }

    public RegionController Create(List<CellController> cells, Action<RegionController, CellController> addFunc, RegionType type = RegionType.Neutral,
      int money = 0)
    {
      _pool.Get(out var instance);
      instance.Type = type;
      foreach (var cell in cells)
        addFunc(instance, cell);

      instance.Money = money;
      _governmentsService.AddRegion(instance);
      _regions.Add(instance);
      return instance;
    }

    public void Destroy(RegionController instance)
    {
      _regions.Remove(instance);
      instance.Clear();
      _pool.Release(instance);
      _governmentsService.RemoveRegion(instance);
    }
  }
}