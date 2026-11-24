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

namespace Client.Project.Player
{
  public class PlayersService : IInitializable
  {
    private readonly List<IPlayerController> _players = new();
    private GovernmentsService _governmentsService;

    public int DifficultyMode { get; set; }

    public int PlayersMode { get; set; }

    public PlayerRegionType FirstPlayer { get; set; }

    public IReadOnlyList<IPlayerController> Players => _players;

    public IPlayerController CurrentPlayer { get; private set; }

    public void Initialize() => _governmentsService = Locator.Get<GovernmentsService>();

    public void Setup(IReadOnlyMap map)
    {
      DifficultyMode = map.DifficultyMode;
      PlayersMode = map.PlayersMode;
      FirstPlayer = map.FirstPlayer;
    }

    public void CreatePlayers(IReadOnlyMap map)
    {
      var players = map.GetInitialPlayers()
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
          CreatePlayer(players[i], i < PlayersMode ? PlayerType.Human : PlayerType.AI);
      }

      if (HasAlivePlayers())
      {
        var playerType = map.CurrentPlayer;
        if (playerType == default || !IsAlive(playerType))
          playerType = GetFirstAlivePlayer().RegionType;
        CurrentPlayer = _players.First(x => x.RegionType == playerType);
      }
    }

    public void ClearPlayers()
    {
      _players.Clear();
      CurrentPlayer = null;
    }

    public bool HasAlivePlayers() => _players.Count(IsAlive) > 0;

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

    public bool IsBotsBattle()
    {
      foreach (var player in _players)
        if (IsAlive(player) && player.Type == PlayerType.Human)
          return false;
      return true;
    }


    public bool MoreThenOneHumanPlayer() => _players.Count(x => IsAlive(x) && x.Type == PlayerType.Human) > 1;

    public void SetFirstAlivePlayerAsCurrent()
    {
      if (HasAlivePlayers())
        CurrentPlayer = GetFirstAlivePlayer();
    }

    public bool SetNextAlivePlayerAsCurrent()
    {
      if (CanGetNextAlivePlayer(CurrentPlayer, out var next))
      {
        CurrentPlayer = next;
        return true;
      }

      return false;
    }

    private void CreatePlayer(PlayerData player, PlayerType type)
    {
      IPlayerController controller;

      if (type == PlayerType.Human)
        controller = new HumanPlayerController(player.RegionType);
      else
        controller = new AIPlayerController(player.RegionType);

      _players.Add(controller);
    }


    private bool IsAlive(IPlayerController player) => IsAlive(player.RegionType);

    private bool IsAlive(RegionType type) => _governmentsService.IsAlive(type);

    private IPlayerController GetFirstAlivePlayer() => _players.FirstOrDefault(IsAlive);

    private bool CanGetNextAlivePlayer(IPlayerController currentPlayer, out IPlayerController next)
    {
      var currentIndex = _players.IndexOf(currentPlayer);
      var maxIndex = _players.Count - 1;
      while (currentIndex < maxIndex)
      {
        next = _players[currentIndex + 1];
        if (IsAlive(next))
          return true;
        currentIndex++;
      }

      next = null;
      return false;
    }
  }
}