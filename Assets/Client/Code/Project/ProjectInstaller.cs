using Client.Code.Gameplay.Map;
using Client.Code.Services;
using Client.Code.Services.Config;
using Client.Code.Services.Progress;
using Client.Code.Services.Scene;
using Client.Code.Services.UnityEvents;
using Zenject;

namespace Client.Code.Project
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