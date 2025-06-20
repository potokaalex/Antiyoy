using ClientCode.Data.Static.Config;
using ClientCode.Services.CanvasService;
using ClientCode.Services.InputService;
using ClientCode.Services.Logger;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;
using ClientCode.UI.Factory;
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
            BindStateMachine();
            BindLog();
            BindUI();
            BindProgress();

            Container.Bind<IStaticDataProvider>().To<StaticDataProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindProgress()
        {
            Container.BindInterfacesTo<ProjectSaveLoader>().AsSingle().WithArguments(_loadDataConfig.Data);
            Container.BindInterfacesTo<MapSaveLoader>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle();
            Container.Bind<IWindowsFactory>().To<WindowsFactory>().AsSingle();
            Container.Bind<ProjectCanvasController>().AsSingle();
            Container.Bind<IWindowsHandler>().To<ProjectWindowsPresenter>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindLog()
        {
            Container.Bind<ILogReceiver>().To<LogReceiver>().AsSingle();
            Container.BindInterfacesTo<LogHandlersRegister>().AsSingle();
            Container.BindInterfacesTo<LoggerByPopup>().AsSingle();
        }

        private void BindStateMachine()
        {
#if DEBUG_STATE_MACHINE
            Container.Bind<IProjectStateMachine>().To<StateMachine>().AsSingle().WithArguments(true);
#else
            Container.Bind<IProjectStateMachine>().To<StateMachine>().AsSingle();
#endif
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}