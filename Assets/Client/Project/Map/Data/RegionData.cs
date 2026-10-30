using System;
using System.Collections.Generic;
using Client.Project.Region;

namespace Client.Project.Map.Data
{
  [Serializable]
  public struct RegionData : IEquatable<RegionData>
  {
    public List<CellData> Cells;
    public RegionType Type;
    public int Money;

    public bool Equals(RegionData other) => Equals(Cells, other.Cells) && Type == other.Type && Money == other.Money;

    public override bool Equals(object obj) => obj is RegionData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Cells, (int)Type, Money);

    public static bool operator ==(RegionData left, RegionData right) => left.Equals(right);

    public static bool operator !=(RegionData left, RegionData right) => !left.Equals(right);
  }
}