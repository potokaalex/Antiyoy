using Client.Code.Core;
using Client.Code.Core.Config;
using Client.Code.Core.Progress;
using Client.Code.Core.Scene;
using Client.Code.Core.UnityEvents;
using Client.Code.Gameplay.Map;
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