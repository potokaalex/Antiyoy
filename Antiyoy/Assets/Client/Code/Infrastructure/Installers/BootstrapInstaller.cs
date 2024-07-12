using ClientCode.Infrastructure.CompositionRoot;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings() => Container.BindInterfacesTo<Bootstrap>();
    }
}