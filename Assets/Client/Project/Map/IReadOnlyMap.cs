using System;
using System.Collections.Generic;
using Client.Project.Map.Data;
using Client.Project.Player;
using Client.Project.Region;

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
    RegionType CurrentPlayer { get; }
    IEnumerable<CellData> GetCells();
    IEnumerable<RegionData> GetRegions();
    IEnumerable<PlayerData> GetInitialPlayers();
  }
}