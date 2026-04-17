using Client.New.Region;
using Client.New.Tile;
using UnityEngine;
using Zenject;

namespace Client.New.Gameplay
{
  public class GameplayInstaller : MonoInstaller
  {
    [SerializeField] private GridController _gridController;
    [SerializeField] private TilemapController _tilemapController;
    [SerializeField] private CameraController _cameraController;

    public override void InstallBindings()
    {
      Container.Bind<TilemapController>().FromInstance(_tilemapController).AsSingle();
      Container.Bind<GridController>().FromInstance(_gridController).AsSingle();
      Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
      Container.Bind<RegionsService>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
    }
  }
}