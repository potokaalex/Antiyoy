using System;

namespace Antiyoy.Gameplay
{
  public class PointYio
  {
    public double x;
    public double y;
    
    public void set(double x, double y) 
    {
      this.x = (float) x;
      this.y = (float) y;
    }

    public void relocateRadial(double distance, double angle) 
    {
      x += distance * Math.Cos(angle);
      y += distance * Math.Sin(angle);
    }
 
    public double distanceTo(PointYio pointYio) => Yio.distance(x, y, pointYio.x, pointYio.y);

    public double angleTo(PointYio pointYio) => Yio.angle(x, y, pointYio.x, pointYio.y);
  }
}