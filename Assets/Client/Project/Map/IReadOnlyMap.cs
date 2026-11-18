using System;
using System.Collections.Generic;
using Client.Project.Map.Data;

namespace Client.Project.Map
{
  public interface IReadOnlyMap
  {
    int Id { get; }
    string Name { get; }
    DateTime CreationTime { get; }
    bool IsAutosave { get; }
    bool IsEditor { get; }
    LevelSize LevelSize { get; }
    int DifficultyMode { get; }
    int PlayersMode { get; }
    PlayerRegionType FirstPlayer { get; }
    IEnumerable<CellData> GetCells();
    IEnumerable<RegionData> GetRegions();
  }
}