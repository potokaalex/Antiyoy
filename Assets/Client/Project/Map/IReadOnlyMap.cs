using System;
using System.Collections.Generic;
using Client.Project.Map.Data;
using UnityEngine;

namespace Client.Project.Map
{
  public interface IReadOnlyMap
  {
    int Id { get; }
    string Name { get; }
    DateTime CreationTime { get; }
    bool IsAutosave { get; }
    bool IsEditor { get; }
    Vector2Int Size { get; }
    IEnumerable<CellData> GetCells();
    IEnumerable<RegionData> GetRegions();
  }
}