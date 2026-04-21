using System.Collections.Generic;
using Client.Region;

namespace Client.Government
{
  public class GovernmentController
  {
    private readonly List<RegionController> _regions = new();

    public IReadOnlyList<RegionController> Regions => _regions;

    public RegionType RegionsType => _regions[0].Type;

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