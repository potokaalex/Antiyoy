using System;
using System.Runtime.CompilerServices;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(CellData other) =>
      RegionType == other.RegionType && Position.Equals(other.Position) && EqualsUnits(other);

    public override bool Equals(object obj) => obj is CellData other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(Position, RegionType, GetUnitHashCode());

    public static bool operator ==(CellData left, CellData right) => left.Equals(right);

    public static bool operator !=(CellData left, CellData right) => !left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool EqualsUnits(CellData other)
    {
      var hasValue = Unit.HasValue;
      var otherHasValue = other.Unit.HasValue;

      if (!hasValue && !otherHasValue)
        return true;
      if (hasValue && otherHasValue)
        return Unit.Value.Equals(other.Unit.Value);
      return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetUnitHashCode() => Unit.HasValue ? HashCode.Combine(Unit.Value, true) : HashCode.Combine(false);
  }
}