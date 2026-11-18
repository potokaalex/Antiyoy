using System;
using System.Collections.Generic;

namespace Client.Project.Map.Data
{
  [Serializable]
  public class MapData
  {
    public int Id = -1;
    public string Name;
    public long CreationTime;
    public List<PlayerData> InitialPlayers = new();
    public List<PlayerData> Players = new();
    public LevelSize LevelSize;
    public int DifficultyMode;
    public int PlayersMode;
    public PlayerRegionType FirstPlayer;
    public bool IsAutosave;
    public bool IsEditor;
  }
}