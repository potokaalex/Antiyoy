using System;
using System.Collections.Generic;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Player;
using Client.Project.Region;

namespace Client.Project.Map
{
  public class MapController : IReadOnlyMap
  {
    private readonly MapsFactory _mapsFactory;

    public MapController(MapData data = null)
    {
      _mapsFactory = Locator.Get<MapsFactory>();
      Data = data;
    }

    public MapData Data { get; set; }

    public int Id => Data.Id;

    public LevelSize LevelSize => Data.LevelSize;

    public string Name { get => Data.Name; set => Data.Name = value; }

    public DateTime CreationTime => DateTime.FromBinary(Data.CreationTime);

    public bool IsAutosave => Data.IsAutosave;

    public bool IsEditor => Data.IsEditor;

    public int DifficultyMode => Data.DifficultyMode;

    public int PlayersMode => Data.PlayersMode;

    public PlayerRegionType FirstPlayer => Data.FirstPlayer;

    public RegionType CurrentPlayer => Data.CurrentPlayer;

    public IEnumerable<CellData> GetCells()
    {
      foreach (var player in Data.Players)
      foreach (var region in player.Regions)
      foreach (var cell in region.Cells)
        yield return cell;
    }

    public IEnumerable<RegionData> GetRegions()
    {
      foreach (var player in Data.Players)
      foreach (var region in player.Regions)
        yield return region;
    }

    public IEnumerable<PlayerData> GetInitialPlayers()
    {
      foreach (var player in Data.InitialPlayers)
        yield return player;
    }

    public void Restart() => Data = _mapsFactory.CreateMapData(Data.InitialPlayers, _mapsFactory.CopyPlayers(Data.InitialPlayers), Data.LevelSize,
      Data.Name, CreationTime, Id, playersMode: Data.PlayersMode, difficultyMode: Data.DifficultyMode, firstPlayer: Data.FirstPlayer);

    public void CopyFrom(MapController map)
    {
      var otherData = map.Data;
      Data = _mapsFactory.CreateMapData(_mapsFactory.CopyPlayers(otherData.InitialPlayers), _mapsFactory.CopyPlayers(otherData.Players),
        otherData.LevelSize, otherData.Name, map.CreationTime, otherData.Id, isAutosave: map.IsAutosave, isEditor: map.IsEditor,
        playersMode: otherData.PlayersMode, difficultyMode: otherData.DifficultyMode, firstPlayer: otherData.FirstPlayer, 
        currentPlayer: otherData.CurrentPlayer);
    }
  }
}