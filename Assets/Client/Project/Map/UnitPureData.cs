using System;
using Client.Project.Unit.Code;

namespace Client.Project.Map
{
  [Serializable]
  public struct UnitPureData : IEquatable<UnitPureData>
  {
    public UnitType Type;
    public bool HasTurns;

    public bool Equals(UnitPureData other) => Type == other.Type && HasTurns == other.HasTurns;

    public override bool Equals(object obj) => obj is UnitPureData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int)Type, HasTurns);

    public static bool operator ==(UnitPureData left, UnitPureData right) => left.Equals(right);

    public static bool operator !=(UnitPureData left, UnitPureData right) => !left.Equals(right);
  }
}