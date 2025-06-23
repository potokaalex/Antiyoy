using Client.Code.Gameplay;
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
using ClientCode.UI.Controllers;
using ClientCode.UI.Windows.Base;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class MapEditorInstaller : MonoInstaller
    {
        public CameraController Camera;
        public GridController Grid;
        public MapEditorWindow Window;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraController>().FromInstance(Camera).AsSingle();
            Container.Bind<EcsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<CellsFactory>().AsSingle();
            Container.Bind<RegionFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridController>().FromInstance(Grid).AsSingle();
            Container.BindInterfacesTo<MapEditorWindow>().FromInstance(Window).AsSingle();

            //1) создать грид с клетками.
            //2) создать мир с клетками, регионами и т.д.

            //Container.BindInterfacesAndSelfTo<StateMachine>().AsSingle();
            //BindFactories();
            //BindProviders();
            //BindUI();
            //BindProgress();

            //Container.Bind<GridManager>().AsSingle();
            //Container.BindInterfacesTo<DelayStartupper<MapEditorEnterState>>().AsSingle();
            //Container.BindInterfacesTo<MapEditorInitializer>();
        }

        private void BindProgress()
        {
            Container.BindInterfacesTo<ProgressActorsRegister>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapKeyFactory>().AsSingle();
        }

        private void BindUI()
        {
            //Container.Bind<IButtonsHandler>().To<MapEditorButtonsPresenter>().AsSingle();
            //Container.Bind<IWindowsHandler>().To<MapEditorWindowsPresenter>().AsSingle();
            //Container.Bind<MapEditorModel>().AsSingle();
            //Container.Bind<MapEditorTouchCellController>().AsSingle();
        }

        private void BindProviders()
        {
            //Container.Bind<MapEditorSceneData>().FromInstance(_sceneData).AsSingle();
            Container.Bind<IEcsProvider>().To<EcsProvider>().AsSingle();
        }

        private void BindFactories()
        {
            //hmm
            Container.BindInterfacesAndSelfTo<EcsFactory>().AsSingle();
            //Container.BindInterfacesAndSelfTo<CellFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileFactoryOld>().AsSingle();
            Container.BindInterfacesAndSelfTo<Gameplay.Region.RegionFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CountryFactory>().AsSingle();
        }
    }
}