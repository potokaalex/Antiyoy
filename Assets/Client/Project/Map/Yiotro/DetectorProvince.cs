using System.Collections.Generic;

namespace Client.Project.Map.Yiotro
{
  public class DetectorProvince
  {
    private readonly List<Hex> _provinceList = new();
    private readonly List<Hex> _propagationList = new();
    private Hex _tempHex;
    private Hex _adjHex;

    public List<Hex> DetectProvince(Hex startHex)
    {
      _provinceList.Clear();
      _propagationList.Clear();
      _propagationList.Add(startHex);

      if (startHex.Fraction == YiotroMapGenerator.NEUTRAL_FRACTION)
      {
        _provinceList.Add(startHex);
        return _provinceList;
      }

      while (_propagationList.Count > 0)
      {
        _tempHex = _propagationList[0];
        _provinceList.Add(_tempHex);
        _propagationList.RemoveAt(0);

        for (var i = 0; i < 6; i++)
        {
          _adjHex = _tempHex.GetAdjacentHex(i);
          if (BelongsToSameProvince(_adjHex))
            _propagationList.Add(_adjHex);
        }
      }

      return _provinceList;
    }

    private bool BelongsToSameProvince(Hex adjHex)
    {
      if (adjHex == null || !adjHex.Active || !adjHex.SameFraction(_tempHex) || _propagationList.Contains(adjHex) || _provinceList.Contains(adjHex))
        return false;
      return true;
    }
  }
}