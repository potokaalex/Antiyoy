using ClientCode.Infrastructure.States.Project;
using ClientCode.Services.Startup;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings() => Container.BindInterfacesTo<AutoStartupper<ProjectLoadState>>().AsSingle();
    }
}