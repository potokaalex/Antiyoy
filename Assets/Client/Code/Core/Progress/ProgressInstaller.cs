using Client.Code.Core.Progress.Actors;
using Zenject;

namespace Client.Code.Core.Progress
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