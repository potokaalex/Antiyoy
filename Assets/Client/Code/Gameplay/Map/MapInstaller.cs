using Zenject;

namespace Client.Code.Gameplay.Map
{
    public class MapInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<MapsContainer>().AsSingle();
            Container.Bind<MapsFactory>().AsSingle();
        }
    }
}