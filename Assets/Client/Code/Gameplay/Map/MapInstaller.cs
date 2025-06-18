using Zenject;

namespace ClientCode.UI.Windows.Writing
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