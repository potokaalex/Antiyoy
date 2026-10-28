using System.Collections.Generic;

namespace Antiyoy.Gameplay
{
  public class DetectorProvince
  {
    private List<Hex> provinceList;
    private List<Hex> propagationList;
    private Hex tempHex;
    private Hex adjHex;

    public DetectorProvince() {
      provinceList = new List<Hex>();
      propagationList = new List<Hex>();
    }
    
    public List<Hex> detectProvince(Hex startHex) {
      provinceList.Clear();

      propagationList.Clear();
      propagationList.Add(startHex);

      if (startHex.fraction == GameRules.NEUTRAL_FRACTION) {
        provinceList.Add(startHex);
        return provinceList;
      }

      while (propagationList.Count > 0) {
        tempHex = propagationList[0];
        provinceList.Add(tempHex);
        propagationList.RemoveAt(0);

        for (int i = 0; i < 6; i++) {
          adjHex = tempHex.getAdjacentHex(i);
          if (belongsToSameProvince(adjHex)) {
            propagationList.Add(adjHex);
          }
        }
      }
      return provinceList;

    }

    private bool belongsToSameProvince(Hex adjHex) {
      if (adjHex == null) return false;
      if (!adjHex.active) return false;
      if (!adjHex.sameFraction(tempHex)) return false;
      if (propagationList.Contains(adjHex)) return false;
      if (provinceList.Contains(adjHex)) return false;

      return true;
    }
  }
}