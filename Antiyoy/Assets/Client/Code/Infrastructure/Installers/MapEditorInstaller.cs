using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class MapEditorInstaller : MonoInstaller
    {
        [SerializeField] private GameplayConfigProvider _configProvider;

        public override void InstallBindings()
        {
            Container.Bind<GameplayConfigProvider>().FromInstance(_configProvider).AsSingle();

            Container.Bind<EcsFactory>().AsSingle();
            Container.Bind<IEcsProvider>().To<EcsProvider>().AsSingle();

            Container.Bind<CellFactory>().AsSingle();
            Container.Bind<TileFactory>().AsSingle();
        }
    }
}