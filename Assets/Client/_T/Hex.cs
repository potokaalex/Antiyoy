namespace Antiyoy.Gameplay
{
  public class Hex
  {
    public int fraction;
    public int previousFraction;
    
    public bool active;
    public int index1;
    public int index2;
    public int objectInside;
    public bool canContainObjects = true;
    
    //Непонятные:
    public int genPotential;//возможно флаг для генерации.
    public int moveZoneNumber;//возможно флаг для генерации.
    public bool genFlag;//возможно флаг для генерации.

    private GameController gameController;

    public Hex(int index1, int index2, GameController gameController)
    {
      this.index1 = index1;
      this.index2 = index2;
      this.gameController = gameController;
    }

    public int numberOfFriendlyHexesNearby() {
      int c = 0;
      for (int dir = 0; dir < 6; dir++) {
        Hex adjHex = getAdjacentHex(dir);
        if (adjHex == null) continue;
        if (adjHex.isNullHex()) continue;
        if (!adjHex.active) continue;
        if (adjHex.isNeutral()) continue;
        if (!adjHex.sameFraction(this)) continue;
        c++;
      }
      return c;
    }
    
    public void setFraction(int fraction) {
      previousFraction = this.fraction;
      this.fraction = fraction;
    }

    public bool isNeutral() => fraction == GameRules.NEUTRAL_FRACTION;

    public Hex getAdjacentHex(int direction) => gameController.fieldManager.adjacentHex(this, direction);

    public bool isNullHex() => index1 == -1 && index2 == -1;

    public bool sameFraction(Hex hex) => sameFraction(hex.fraction);
    
    public bool sameFraction(int fraction) => this.fraction == fraction;

    public bool noProvincesNearby() {
      if (numberOfFriendlyHexesNearby() > 0) return false;
      for (int i = 0; i < 6; i++) {
        Hex adjHex = getAdjacentHex(i);
        if (adjHex.active && adjHex.numberOfFriendlyHexesNearby() > 0) return false;
      }
      return true;
    }
    
    public int numberOfActiveHexesNearby() => numberOfFriendlyHexesNearby() + howManyEnemyHexesNear();

    public bool containsObject() => objectInside > 0;

    public PointYio getPos()
    {
      //возможно позиция в мировом пространстве.
      throw new System.NotImplementedException();
    }

    public int howManyEnemyHexesNear() {
      int c = 0;
      for (int i = 0; i < 6; i++) {
        Hex adjHex = getAdjacentHex(i);
        if (adjHex.active && !adjHex.sameFraction(this)) c++;
      }
      return c;
    }

    public bool isNearWater() {
      if (!this.active) return false;
      for (int i = 0; i < 6; i++) {
        if (!gameController.fieldManager.adjacentHex(this, i).active) return true;
      }
      return false;
    }

    public void setObjectInside(int objectInside) => this.objectInside = objectInside;
  }
}