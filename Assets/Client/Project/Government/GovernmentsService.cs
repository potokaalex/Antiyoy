using System.Collections.Generic;
using Client.Project.Region;
using UnityEngine.Pool;

namespace Client.Project.Government
{
  public class GovernmentsService
  {
    private readonly Dictionary<RegionType, GovernmentController> _governments = new();

    public void AddRegion(RegionController region)
    {
      if (!_governments.TryGetValue(region.Type, out var government))
        government = _governments[region.Type] = new GovernmentController(region.Type);
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

    public IEnumerable<GovernmentController> GetAll()
    {
      foreach (var government in _governments.Values)
        yield return government;
    }

    public void GetAllAlive(List<GovernmentController> outList)
    {
      outList.Clear();
      foreach (var government in _governments.Values)
        if (government.IsAlive)
          outList.Add(government);
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
      foreach (var government in _governments.Values)
        if (government.RegionsType == regionType)
          return government;

      return null;
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
  }
}