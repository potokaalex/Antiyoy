using ClientCode.Data.Scene;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.StateMachine;
using ClientCode.UI;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map.SaveLoad;
using ClientCode.UI.Handlers;
using ClientCode.UI.Handlers.MapEditor;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;
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
            BindUI();
            BindProgress();
            
            Container.Bind<CameraController>().AsSingle().WithArguments(_sceneData.Camera);
        }
        
        private void BindProgress()
        {
            Container.BindInterfacesTo<ProgressActorsRegister>().AsSingle();
            Container.BindInterfacesTo<MapLoader>().AsSingle();
            Container.BindInterfacesTo<MapKeySaver>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle().WithArguments(_sceneData.UIRoot);
            Container.Bind<WindowsFactory>().AsSingle();
            Container.Bind<ILoadButtonHandler>().To<MapEditorButtonsHandler>().AsCached();
            Container.Bind<IMapSaveLoadButtonHandler>().To<MapEditorButtonsHandler>().AsCached();
            Container.Bind<IWindowsHandler>().To<MapEditorWindowsHandler>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }

        private void BindProviders()
        {
            Container.Bind<MapEditorSceneData>().FromInstance(_sceneData).AsSingle();
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