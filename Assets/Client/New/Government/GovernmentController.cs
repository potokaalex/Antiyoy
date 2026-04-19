using System.Collections.Generic;
using Client.New.Region;

namespace Client.New.Government
{
  public class GovernmentController
  {
    private readonly List<RegionController> _regions = new();

    public IReadOnlyList<RegionController> Regions => _regions;

    public void AddRegion(RegionController region)
    {
      _regions.Add(region);
    }

    public void RemoveRegion(RegionController region)
    {
      _regions.Remove(region);
    }
  }
}