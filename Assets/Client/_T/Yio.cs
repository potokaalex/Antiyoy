using System;

namespace Antiyoy.Gameplay
{
  public class Yio
  {
    public static double distance(double x1, double y1, double x2, double y2) {
      return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
    }

    public static double angle(double x1, double y1, double x2, double y2) {
      if (x1 == x2) {
        if (y2 > y1) return 0.5 * Math.PI;
        if (y2 < y1) return 1.5 * Math.PI;
        return 0;
      }
      if (x2 >= x1) return Math.Atan((y2 - y1) / (x2 - x1));
      else return Math.PI + Math.Atan((y2 - y1) / (x2 - x1));
    }
  }
}