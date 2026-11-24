using System.Collections.Generic;
using System.Linq;
using Client.Project.Region;
using UnityEngine.Pool;

namespace Client.Project.Government
{
  public class GovernmentsService
  {
    private readonly Dictionary<RegionType, GovernmentController> _governments = new();

    public void AddRegion(RegionController region) => Get(region.Type).AddRegion(region);

    public void RemoveRegion(RegionController region)
    {
      if (_governments.TryGetValue(region.Type, out var government))
        government.RemoveRegion(region);
    }

    public IEnumerable<GovernmentController> GetAll()
    {
      foreach (var government in _governments.Values)
        yield return government;
    }

    public bool IsAlive(RegionType regionType)
    {
      foreach (var government in _governments.Values)
        if (government.RegionsType == regionType && government.IsAlive)
          return true;

      return false;
    }

    public GovernmentController Get(RegionType regionType)
    {
      if (!_governments.TryGetValue(regionType, out var government))
        government = _governments[regionType] = new GovernmentController(regionType);
      return government;
    }

    public void Clear() => _governments.Clear();

    public void ClearEmpty()
    {
      using (ListPool<GovernmentController>.Get(out var governments))
      {
        foreach (var government in _governments.Values)
          if (government.Regions.Count < 1)
            governments.Add(government);

        foreach (var government in governments)
          _governments.Remove(government.RegionsType);
      }
    }

    public int CountNonEmptyWithoutNeutral() => GetAll().Count(x => x.Regions.Count > 0 && x.RegionsType != RegionType.Neutral);
  }
}