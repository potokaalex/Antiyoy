using ClientCode.Data.Static.Config;
using ClientCode.Services.Progress;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;
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

            Container.Bind<IStaticDataProvider>().To<StaticDataProvider>().AsSingle();
            Container.Bind<IProgressDataSaveLoader>().To<ProgressDataSaveLoader>().AsSingle().WithArguments(_loadDataConfig.Data);
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}