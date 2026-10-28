using System;
using System.Collections.Generic;
using Client.Project.Region;

namespace Client.Project.Map
{
  [Serializable]
  public struct RegionPureData : IEquatable<RegionPureData>
  {
    public List<CellPureData> Cells;
    public RegionType Type;
    public int Money;

    public bool Equals(RegionPureData other) => Equals(Cells, other.Cells) && Type == other.Type && Money == other.Money;

    public override bool Equals(object obj) => obj is RegionPureData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Cells, (int)Type, Money);

    public static bool operator ==(RegionPureData left, RegionPureData right) => left.Equals(right);

    public static bool operator !=(RegionPureData left, RegionPureData right) => !left.Equals(right);
  }
}