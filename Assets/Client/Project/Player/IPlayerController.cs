using Client.Project.Region;

namespace Client.Project.Player
{
  public interface IPlayerController
  {
    PlayerType Type { get; }
    RegionType RegionType { get; }
    void StartTurn();
    void EndTurn();
    void Pause();
    void Tick();
  }
}