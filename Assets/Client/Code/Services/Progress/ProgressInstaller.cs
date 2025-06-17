using Client.Code.Services.Progress.Actors;
using Zenject;

namespace Client.Code.Services.Progress
{
    public class ProgressInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProgressActorsRegister>().AsSingle().CopyIntoAllSubContainers();
            Container.BindInterfacesAndSelfTo<ProgressController>().AsSingle();
        }
    }
}