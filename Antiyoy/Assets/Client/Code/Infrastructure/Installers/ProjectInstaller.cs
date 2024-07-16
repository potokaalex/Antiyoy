using ClientCode.Data.Static.Config;
using ClientCode.Services.CanvasService;
using ClientCode.Services.Logger;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;
using ClientCode.UI;
using ClientCode.UI.Handlers;
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
            BindStateMachine();
            BindLog();
            BindUI();

            Container.Bind<IStaticDataProvider>().To<StaticDataProvider>().AsSingle();
            Container.Bind<IProgressDataSaveLoader>().To<ProgressDataSaveLoader>().AsSingle().WithArguments(_loadDataConfig.Data);
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle();
            Container.Bind<ProjectCanvasController>().AsSingle();
            Container.Bind<IWindowsHandler>().To<ProjectWindowsHandler>().AsSingle();
        }

        private void BindLog()
        {
            Container.Bind<ILogReceiver>().To<LogReceiver>().AsSingle();
            Container.BindInterfacesTo<LogHandlersRegister>().AsSingle();
            Container.BindInterfacesTo<LoggerByPopup>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}