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
            Container.BindInterfacesAndSelfTo<EcsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<CellsFactory>().AsSingle();
            BindRegion();
            Container.BindInterfacesAndSelfTo<GridController>().FromInstance(Grid).AsSingle();
            Container.BindInterfacesTo<MapEditorWindow>().FromInstance(Window).AsSingle();
        }

        private void BindRegion()
        {
            Container.BindInterfacesAndSelfTo<RegionFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<RegionCreator>().AsSingle();
            Container.BindInterfacesAndSelfTo<RegionJoiner>().AsSingle();
            Container.BindInterfacesAndSelfTo<RegionDivider>().AsSingle();
            Container.Bind<RegionsContainer>().AsSingle();
        }
    }
}