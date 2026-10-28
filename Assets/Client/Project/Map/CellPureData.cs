using System;
using Client.Project.Hex;
using Client.Project.Region;

namespace Client.Project.Map
{
  [Serializable]
  public struct CellPureData : IEquatable<CellPureData>
  {
    public HexCoordinates Position;
    public RegionType RegionType;
    public UnitPureData? Unit;

    public bool Equals(CellPureData other) =>
      Position.Equals(other.Position) && Nullable.Equals(Unit, other.Unit) && RegionType.Equals(other.RegionType);

    public override bool Equals(object obj) => obj is CellPureData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Position, Unit, RegionType);

    public static bool operator ==(CellPureData left, CellPureData right) => left.Equals(right);

    public static bool operator !=(CellPureData left, CellPureData right) => !left.Equals(right);
  }
}