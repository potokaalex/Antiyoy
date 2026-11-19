using System.Collections.Generic;
using System.Linq;
using Client.Gameplay.Player;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Map.Data;
using Client.Project.Region;

namespace Client.Project.Player
{
  public class PlayersService : IInitializable
  {
    private readonly List<IPlayerController> _players = new();
    private GovernmentsService _governmentsService;

    public int DifficultyMode { get; set; }

    public int PlayersMode { get; set; }

    public PlayerRegionType FirstPlayer { get; set; } //todo: editor only!

    public IReadOnlyList<IPlayerController> Players => _players;

    public void Initialize() => _governmentsService = Locator.Get<GovernmentsService>();

    public void Setup(IReadOnlyMap map)
    {
      DifficultyMode = map.DifficultyMode;
      PlayersMode = map.PlayersMode;
      FirstPlayer = map.FirstPlayer;

      foreach (var player in map.GetPlayers())
        if(player.RegionType != RegionType.Neutral)
          CreatePlayer(player);
    }

    public void Clear()
    {
      _players.Clear();
      DifficultyMode = 0;
      PlayersMode = 0;
      FirstPlayer = default;
    }

    public int PlayersCount() => _governmentsService.GetAll().Count(x => x.Regions.Count > 0 && x.RegionsType != RegionType.Neutral);//todo

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

    private void CreatePlayer(PlayerData player)
    {
      IPlayerController controller;

      if (player.Type == PlayerType.OnDevice)
        controller = new PlayerController(player.RegionType);
      else if (player.Type == PlayerType.Online)
        controller = new OnlinePlayerController(player.RegionType);
      else
        controller = new AIPlayerController(player.RegionType);

      _players.Add(controller);
    }

    private bool IsAlive(IPlayerController player) => _governmentsService.IsAlive(player.RegionType);
  }
}