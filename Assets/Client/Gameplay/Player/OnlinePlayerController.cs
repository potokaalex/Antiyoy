using Client.Gameplay.Gameplay;
using Client.Project.Infrastructure;
using Client.Project.Player;
using Client.Project.Region;
using UnityEngine;

namespace Client.Gameplay.Player
{
  public class OnlinePlayerController : IPlayerController
  {
    private readonly GameplayController _gameplayController;

    public RegionType RegionType { get; }

    public OnlinePlayerController(RegionType regionType)
    {
      RegionType = regionType;
      _gameplayController = Locator.Get<GameplayController>();
    }

    public void StartTurn()
    {
      Debug.Log($"OnlinePlayerTurn: {RegionType}");
      _gameplayController.NextTurn();
    }

    public void EndTurn()
    {
    }

    public void Pause()
    {
    }

    public void Tick()
    {
    }
  }
}