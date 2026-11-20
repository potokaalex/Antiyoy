using System;
using System.Runtime.CompilerServices;
using Client.Project.Unit.Code;

namespace Client.Project.Map.Data
{
  [Serializable]
  public struct UnitData : IEquatable<UnitData>
  {
    public UnitType Type;
    public bool HasTurns;

    public UnitData(UnitType type, bool hasTurns = false)
    {
      Type = type;
      HasTurns = hasTurns;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(UnitData other) => Type == other.Type && HasTurns == other.HasTurns;

    public override bool Equals(object obj) => obj is UnitData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int)Type, HasTurns);

    public static bool operator ==(UnitData left, UnitData right) => left.Equals(right);

    public static bool operator !=(UnitData left, UnitData right) => !left.Equals(right);
  }
}