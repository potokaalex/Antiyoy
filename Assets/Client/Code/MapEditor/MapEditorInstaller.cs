using Client.Code.Gameplay;
using Client.Code.Gameplay.Cell;
using Client.Code.Gameplay.Region;
using Zenject;

namespace Client.Code.MapEditor
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
        }
    }
}