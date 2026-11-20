using System;
using System.Collections.Generic;
using System.Linq;
using Client.Gameplay.Player;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Map.Data;
using Client.Project.Region;
using Client.Project.Yiotro;
using UnityEngine;

namespace Client.Project.Player
{
  public class PlayersService : IInitializable
  {
    private readonly List<IPlayerController> _players = new();
    private GovernmentsService _governmentsService;
    private PlayerRegionType _fp;

    public int DifficultyMode { get; set; }

    public int PlayersMode { get; set; }

    public PlayerRegionType FirstPlayer
    {
      get => _fp;
      set
      {
        _fp = value;
        Debug.Log($"Set {_fp.IsRandom}, {_fp.RegionType}");
      }
    } //todo: editor only!

    public IReadOnlyList<IPlayerController> Players => _players;

    public void Initialize() => _governmentsService = Locator.Get<GovernmentsService>();

    public void Setup(IReadOnlyMap map)
    {
      DifficultyMode = map.DifficultyMode;
      PlayersMode = map.PlayersMode;
      FirstPlayer = map.FirstPlayer;
    }

    public void CreatePlayers(IReadOnlyMap map)
    {
      var players = map.GetPlayers()
        .OrderBy(x => x.RegionType == RegionType.Neutral ? int.MaxValue : YiotroUtilities.RegionTypeToEditorColorCode(x.RegionType)).ToList();

      if (FirstPlayer.IsRandom || players.All(x => x.RegionType != FirstPlayer.RegionType))
        throw new Exception($"Invalid firstPlayer value: {FirstPlayer.IsRandom}, {FirstPlayer.RegionType}");

      if (players.Count > 0)
      {
        var movement = players.FindIndex(x => x.RegionType == FirstPlayer.RegionType);
        var shift = (movement % players.Count + players.Count) % players.Count;
        var temp = players.GetRange(0, shift);
        players.RemoveRange(0, shift);
        players.AddRange(temp);

        var neutralIndex = players.FindIndex(x => x.RegionType == RegionType.Neutral);
        if (neutralIndex != -1)
          players.RemoveAt(neutralIndex);

        for (var i = 0; i < players.Count; i++)
          CreatePlayer(players[i], i < PlayersMode ? PlayerType.OnDevice : PlayerType.AI);
      }
    }

    public void ClearPlayers() => _players.Clear();

    public bool HasPlayers() => _players.Count > 0;

    public bool IsOneAlive(out IPlayerController alive)
    {
      if (_players.Count(IsAlive) == 1)
      {
        alive = _players.First(IsAlive);
        return true;
      }

      alive = null;
      return false;
    }

    public bool CanGetNextPlayer(IPlayerController currentPlayer, out IPlayerController next)
    {
      var currentIndex = _players.IndexOf(currentPlayer);
      var maxIndex = _players.Count - 1;
      if (currentIndex < maxIndex)
      {
        next = _players[currentIndex + 1];
        return true;
      }

      next = null;
      return false;
    }

    private void CreatePlayer(PlayerData player, PlayerType type)
    {
      IPlayerController controller;

      if (type == PlayerType.OnDevice)
        controller = new PlayerController(player.RegionType);
      else
        controller = new AIPlayerController(player.RegionType);

      _players.Add(controller);
    }

    private bool IsAlive(IPlayerController player) => _governmentsService.IsAlive(player.RegionType);
  }
}