using System.Collections.Generic;
using Client.New.Government;
using Client.New.Infrastructure;

namespace Client.New.Region
{
  public class RegionsFactory : IInitializable
  {
    private GovernmentsService _governmentsService;

    public void Initialize()
    {
      _governmentsService = Locator.Get<GovernmentsService>();
    }

    public RegionController Create(List<CellController> cells = null, RegionType type = RegionType.Default)
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