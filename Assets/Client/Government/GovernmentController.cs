using System.Collections.Generic;
using Client.Region;

namespace Client.Government
{
  public class GovernmentController
  {
    private readonly List<RegionController> _regions = new();

    public IReadOnlyList<RegionController> Regions => _regions;

    public RegionType RegionsType => _regions[0].Type;

    public bool IsAlive
    {
      get
      {
        foreach (var region in _regions)
          if (region.IsAlive)
            return true;
        return false;
      }
    }

    public void AddRegion(RegionController region) => _regions.Add(region);

    public void RemoveRegion(RegionController region) => _regions.Remove(region);
  }
}