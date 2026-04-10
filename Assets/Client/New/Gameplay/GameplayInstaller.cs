using Client.New.Region;
using UnityEngine;
using Zenject;

namespace Client.New.Gameplay
{
  public class GameplayInstaller : MonoInstaller
  {
    [SerializeField] private GridController _grid;
    [SerializeField] private CameraController _camera;

    public override void InstallBindings()
    {
      Container.Bind<GridController>().FromInstance(_grid).AsSingle();
      Container.Bind<CameraController>().FromInstance(_camera).AsSingle();
      Container.Bind<RegionsService>().AsSingle();
      Container.BindInterfacesAndSelfTo<MapController>().AsSingle();
    }
  }
}