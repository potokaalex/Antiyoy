using System.Collections.Generic;
using Client.New.Government;
using Client.New.Infrastructure;
using UnityEngine.Pool;

namespace Client.New.Region
{
  public class RegionsFactory : IInitializable
  {
    private GovernmentsService _governmentsService;
    private ObjectPool<RegionController> _pool;

    public void Initialize()
    {
      _governmentsService = Locator.Get<GovernmentsService>();
      _pool = new ObjectPool<RegionController>(() => new RegionController(this));
    }

    public RegionController Create(CellController cell, RegionType type = RegionType.Neutral)
    {
      using (ListPool<CellController>.Get(out var list))
      {
        list.Add(cell);
        return Create(list, type);
      }
    }

    public RegionController Create(List<CellController> cells, RegionType type = RegionType.Neutral)
    {
      _pool.Get(out var instance);
      instance.Type = type;
      foreach (var cell in cells)
        instance.Add(cell);

      _governmentsService.AddRegion(instance);
      return instance;
    }

    public void Destroy(RegionController instance)
    {
      _pool.Release(instance);
      _governmentsService.RemoveRegion(instance);
    }
  }
}