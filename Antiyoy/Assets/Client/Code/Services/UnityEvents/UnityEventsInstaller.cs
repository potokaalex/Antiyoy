using Zenject;

namespace Client.Code.Services.UnityEvents
{
    public class UnityEventsInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<UnityEventsSender>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesTo<UnityEventsRegister>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}