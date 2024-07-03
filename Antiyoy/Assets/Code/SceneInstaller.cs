using Code.Cell;
using Code.Ecs;
using Code.Tile;
using UnityEngine;
using Zenject;

namespace Code
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private ConfigProvider _configProvider;

        public override void InstallBindings()
        {
            Container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
            
            Container.Bind<EcsFactory>().AsSingle();
            Container.Bind<IEcsProvider>().To<EcsProvider>().AsSingle();
            
            Container.Bind<CellFactory>().AsSingle();
            Container.Bind<TileFactory>().AsSingle();
        }
    }
}