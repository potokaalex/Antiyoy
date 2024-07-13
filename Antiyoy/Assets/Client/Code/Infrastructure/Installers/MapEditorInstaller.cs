using ClientCode.Data.Scene;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Infrastructure.Startup;
using ClientCode.Services.SceneDataProvider;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using ClientCode.UI.Presenters;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class MapEditorInstaller : MonoInstaller
    {
        [SerializeField] private MapEditorSceneData _sceneData;

        public override void InstallBindings()
        {
            BindStateMachine();
            BindFactories();
            BindProviders();

            Container.Bind<ILoadSceneButtonHandler>().To<MapEditorPresenter>().AsSingle();
            Container.BindInterfacesTo<MapEditorStartup>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }

        private void BindProviders()
        {
            Container.Bind<ISceneDataProvider<MapEditorSceneData>>().To<SceneDataProvider<MapEditorSceneData>>().AsSingle()
                .WithArguments(_sceneData);
            Container.Bind<IEcsProvider>().To<EcsProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<EcsFactory>().AsSingle();
            Container.Bind<CellFactory>().AsSingle();
            Container.Bind<TileFactory>().AsSingle();
        }
    }
}