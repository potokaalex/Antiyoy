using System.Collections.Generic;
using Client.New.Government;
using Client.New.Infrastructure;
using UnityEngine.Pool;

namespace Client.New.Region
{
  public class RegionsFactory : IInitializable
  {
    private GovernmentsService _governmentsService;

    public void Initialize()
    {
      _governmentsService = Locator.Get<GovernmentsService>();
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
      //todo: pool.
      var instance = new RegionController(this, cells, type);
      _governmentsService.AddRegion(instance);
      return instance;
    }

    public void Destroy(RegionController instance)
    {
      //todo: pool.
      _governmentsService.RemoveRegion(instance);
    }
  }
}