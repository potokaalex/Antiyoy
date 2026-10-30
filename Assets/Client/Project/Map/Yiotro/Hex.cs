namespace Client.Project.Map.Yiotro
{
  public class Hex
  {
    public readonly int Index1;
    public readonly int Index2;
    public bool Active;
    public int Fraction;
    public int ObjectInside;
    public int GenPotential;
    public int MoveZoneNumber;
    public bool GenFlag;
    private readonly FieldManager _fieldManager;

    public Hex(int index1, int index2, FieldManager fieldManager)
    {
      Index1 = index1;
      Index2 = index2;
      _fieldManager = fieldManager;
    }

    public int NumberOfFriendlyHexesNearby()
    {
      var c = 0;
      for (var dir = 0; dir < 6; dir++)
      {
        var adjHex = GetAdjacentHex(dir);
        if (adjHex == null || adjHex.IsNullHex() || !adjHex.Active || adjHex.IsNeutral() || !adjHex.SameFraction(this))
          continue;
        c++;
      }

      return c;
    }

    public void SetFraction(int fraction) => Fraction = fraction;

    public bool IsNeutral() => Fraction == YiotroMapGenerator.NEUTRAL_FRACTION;

    public Hex GetAdjacentHex(int direction) => _fieldManager.AdjacentHex(this, direction);

    public bool IsNullHex() => Index1 == -1 && Index2 == -1;

    public bool SameFraction(Hex hex) => SameFraction(hex.Fraction);

    public bool SameFraction(int fraction) => Fraction == fraction;

    public bool NoProvincesNearby()
    {
      if (NumberOfFriendlyHexesNearby() > 0)
        return false;
      for (var i = 0; i < 6; i++)
      {
        var adjHex = GetAdjacentHex(i);
        if (adjHex.Active && adjHex.NumberOfFriendlyHexesNearby() > 0)
          return false;
      }

      return true;
    }

    public int NumberOfActiveHexesNearby() => NumberOfFriendlyHexesNearby() + HowManyEnemyHexesNear();

    public bool ContainsObject() => ObjectInside > 0;

    public bool IsNearWater()
    {
      if (!Active)
        return false;
      for (var i = 0; i < 6; i++)
        if (!_fieldManager.AdjacentHex(this, i).Active)
          return true;
      return false;
    }

    public void SetObjectInside(int objectInside) => ObjectInside = objectInside;

    private int HowManyEnemyHexesNear()
    {
      var c = 0;
      for (var i = 0; i < 6; i++)
      {
        var adjHex = GetAdjacentHex(i);
        if (adjHex.Active && !adjHex.SameFraction(this))
          c++;
      }

      return c;
    }
  }
}