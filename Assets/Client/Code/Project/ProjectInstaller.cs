using Client.Code.Services;
using Client.Code.Services.Config;
using Client.Code.Services.Progress;
using Client.Code.Services.UnityEvents;
using ClientCode.Services.SceneLoader;
using ClientCode.UI.Windows.Writing;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<UnityEventsInstaller>();
            Container.Bind<Instantiator>().AsSingle().CopyIntoAllSubContainers();
            Container.BindInterfacesAndSelfTo<ConfigsController>().AsSingle();
            Container.BindInterfacesTo<ProgressController>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectManager>().AsSingle();
            Container.Install<MapInstaller>();
        }
    }
}