using System.Collections.Generic;
using Client.Region;

namespace Client.Government
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
        government.RemoveRegion(region);
    }

    public void GetAll(List<GovernmentController> outList)
    {
      outList.Clear();
      outList.AddRange(_governments.Values);
    }

    public void GetAllAlive(List<GovernmentController> outList)
    {
      outList.Clear();
      foreach (var government in _governments.Values)
        if (government.IsAlive)
          outList.Add(government);
    }
  }
}