using System.Collections.Generic;
using Client.New.Region;

namespace Client.New.Government
{
  public class GovernmentsService
  {
    private readonly Dictionary<RegionType, GovernmentController> _governments = new();

    public void AddRegion(RegionController region)
    {
      if (!_governments.TryGetValue(region.Type, out var government))
        government = _governments[region.Type] = new GovernmentController();

      government.AddRegion(region);
    }

    public void RemoveRegion(RegionController region)
    {
      if (_governments.TryGetValue(region.Type, out var government))
      {
        government.RemoveRegion(region);
        if (government.Regions.Count == 0)
          _governments.Remove(region.Type);
      }
    }

    public void GetAll(List<GovernmentController> outList)
    {
      outList.Clear();
      outList.AddRange(_governments.Values);
    }
  }
}