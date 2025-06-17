using ClientCode.Client.Code.Services.StateMachineCode;
using ClientCode.Data.Scene;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Countries;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Map.Factory;
using ClientCode.UI.Buttons.Base;
using ClientCode.UI.Controllers;
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
            Container.BindInterfacesAndSelfTo<StateMachine>().AsSingle();
            BindFactories();
            BindProviders();
            BindUI();
            BindProgress();

            Container.Bind<GridManager>().AsSingle();
            //Container.BindInterfacesTo<DelayStartupper<MapEditorEnterState>>().AsSingle(); TODO
        }

        private void BindProgress()
        {
            Container.BindInterfacesTo<ProgressActorsRegister>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapKeyFactory>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<IButtonsHandler>().To<MapEditorButtonsPresenter>().AsSingle();
            Container.Bind<IWindowsHandler>().To<MapEditorWindowsPresenter>().AsSingle();
            Container.Bind<MapEditorModel>().AsSingle();
            Container.Bind<MapEditorTouchCellController>().AsSingle();
            Container.Bind<CameraController>().AsSingle().WithArguments(_sceneData.Camera);
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
            Container.BindInterfacesAndSelfTo<CountryFactory>().AsSingle();
        }
    }
}