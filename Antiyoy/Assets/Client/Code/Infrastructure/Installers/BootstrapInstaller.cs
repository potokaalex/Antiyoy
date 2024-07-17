using ClientCode.Infrastructure.States.Project;
using ClientCode.Services.StateMachine;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings() => Container.BindInterfacesTo<StateStartuper<ProjectLoadState>>().AsSingle();
    }
}