using System.Runtime.CompilerServices;

namespace Client.Project.Yiotro
{
  public class Hex
  {
    private readonly Hex[] _adjacentHexes = new Hex[6];
    private readonly FieldManager _fieldManager;
    public readonly int Index1;
    public readonly int Index2;
    private readonly bool _isNullHex;
    public bool Active;
    public int Fraction;
    public int ObjectInside;
    public int GenPotential;
    public int MoveZoneNumber;
    public bool GenFlag;

    public Hex(int index1, int index2, FieldManager fieldManager)
    {
      Index1 = index1;
      Index2 = index2;
      _fieldManager = fieldManager;
      _isNullHex = Index1 == -1 && Index2 == -1;
    }

    public void Setup()
    {
      Active = false;
      Fraction = 0;
      ObjectInside = 0;
      GenPotential = 0;
      MoveZoneNumber = 0;
      GenFlag = false;

      for (var i = 0; i < 6; i++)
        _adjacentHexes[i] = _fieldManager.AdjacentHex(this, i);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Hex GetAdjacentHex(int direction) => _adjacentHexes[direction];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsNullHex() => _isNullHex;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool SameFraction(Hex hex) => SameFraction(hex.Fraction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool SameFraction(int fraction) => Fraction == fraction;

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