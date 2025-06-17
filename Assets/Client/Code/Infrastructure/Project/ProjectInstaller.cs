using Client.Code.Services.Config;
using Client.Code.Services.Progress;
using Client.Code.Services.UnityEvents;
using ClientCode.Client.Code.Services.StateMachineCode;
using ClientCode.Services.InputService;
using ClientCode.Services.Logger;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.SceneLoader;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<UnityEventsInstaller>();
            Container.BindInterfacesAndSelfTo<StateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConfigsController>().AsSingle();
            Container.BindInterfacesTo<ProgressController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<ProjectManager>().AsSingle();
        }

        private void BindLog()
        {
            //TODO will see
            Container.Bind<ILogReceiver>().To<LogReceiver>().AsSingle();
            Container.BindInterfacesTo<LogHandlersRegister>().AsSingle();
            Container.BindInterfacesTo<LoggerByPopup>().AsSingle();
        }
    }
}