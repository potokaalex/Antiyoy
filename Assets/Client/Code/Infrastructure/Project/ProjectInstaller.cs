using Client.Code.Services.Config;
using Client.Code.Services.UnityEvents;
using ClientCode.Client.Code.Services.StateMachineCode;
using ClientCode.Data.Static.Config;
using ClientCode.Services.CanvasService;
using ClientCode.Services.InputService;
using ClientCode.Services.Logger;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;
using ClientCode.UI.Presenters;
using ClientCode.UI.Windows.Base;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ProjectLoadDataConfig _loadDataConfig;

        public override void InstallBindings()
        {
            Container.Install<UnityEventsInstaller>();
            Container.BindInterfacesAndSelfTo<StateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConfigsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();

            BindLog(); //hmm. -will see
            BindProgress();

            
            //remove
            Container.Bind<IUpdater>().To<Updater>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindProgress()
        {
            Container.BindInterfacesTo<ProjectSaveLoader>().AsSingle().WithArguments(_loadDataConfig.Data);
            Container.BindInterfacesTo<MapSaveLoader>().AsSingle();
        }

        private void BindLog()
        {
            Container.Bind<ILogReceiver>().To<LogReceiver>().AsSingle();
            Container.BindInterfacesTo<LogHandlersRegister>().AsSingle();
            Container.BindInterfacesTo<LoggerByPopup>().AsSingle();
        }
    }
}