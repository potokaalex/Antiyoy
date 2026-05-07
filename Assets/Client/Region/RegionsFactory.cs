using System.Collections.Generic;
using Client.Government;
using Client.Infrastructure;
using UnityEngine.Pool;

namespace Client.Region
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
      instance.CreateCapital();

      _governmentsService.AddRegion(instance);
      _regions.Add(instance);
      return instance;
    }

    public void Destroy(RegionController instance)
    {
      _regions.Remove(instance);
      instance.Money = 0;
      _pool.Release(instance);
      _governmentsService.RemoveRegion(instance);
    }
  }
}