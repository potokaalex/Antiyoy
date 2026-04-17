using Client.New.Infrastructure;
using Client.New.Region;
using Client.New.Tile;
using UnityEngine;

namespace Client.New.Gameplay
{
  public class GameplayInstaller : MonoInstaller
  {
    [SerializeField] private TilemapController _tilemapController;
    [SerializeField] private GridController _gridController;
    [SerializeField] private CameraController _cameraController;

    protected override void Install()
    {
      Locator.Set(_tilemapController);
      Locator.Set(_gridController);
      Locator.Set(_cameraController);
      Locator.Set(new RegionsService());
      Locator.Set(new GameController());
    }
  }
}