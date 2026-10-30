using System;
using Client.Project.Hex;
using Client.Project.Region;

namespace Client.Project.Map.Data
{
  [Serializable]
  public struct CellData : IEquatable<CellData>
  {
    public HexCoordinates Position;
    public RegionType RegionType;
    public UnitData? Unit;

    public bool Equals(CellData other) =>
      Position.Equals(other.Position) && Nullable.Equals(Unit, other.Unit) && RegionType.Equals(other.RegionType);

    public override bool Equals(object obj) => obj is CellData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Position, Unit, RegionType);

    public static bool operator ==(CellData left, CellData right) => left.Equals(right);

    public static bool operator !=(CellData left, CellData right) => !left.Equals(right);
  }
}