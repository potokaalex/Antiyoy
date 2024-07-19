using ClientCode.Data.Scene;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Map.Factory;
using ClientCode.Services.Startup;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Controllers;
using ClientCode.UI.Factory;
using ClientCode.UI.Models;
using ClientCode.UI.Presenters.MapEditor;
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
            Container.BindInterfacesTo<DelayStartupper<MapEditorEnterState>>().AsSingle();
        }

        private void BindProgress()
        {
            Container.BindInterfacesTo<ProgressActorsRegister>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapKeyFactory>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle().WithArguments(_sceneData.UIRoot);
            Container.Bind<IWindowsFactory>().To<WindowsFactory>().AsSingle();
            Container.Bind<IButtonsHandler>().To<MapEditorButtonsPresenter>().AsSingle();
            Container.Bind<IWindowsHandler>().To<MapEditorWindowsPresenter>().AsSingle();
            Container.Bind<MapEditorModel>().AsSingle();
            Container.Bind<MapEditorTouchCellController>().AsSingle();
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
            Container.BindInterfacesAndSelfTo<EcsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CellFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<RegionFactory>().AsSingle();
        }
    }
}