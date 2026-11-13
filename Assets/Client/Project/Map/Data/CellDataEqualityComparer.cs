using System.Collections.Generic;

namespace Client.Project.Map.Data
{
  public class CellDataEqualityComparer : IEqualityComparer<CellData>
  {
    public static readonly CellDataEqualityComparer Instance = new();

    public bool Equals(CellData x, CellData y) => x.Equals(y);

    public int GetHashCode(CellData obj) => obj.GetHashCode();
  }
}