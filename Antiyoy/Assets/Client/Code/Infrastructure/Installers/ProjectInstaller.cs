using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;
using ClientCode.Data.Static;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.SaveLoader;
using ClientCode.Services.SaveLoader.Progress;
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
        [SerializeField] private ProjectLoadData _loadData;

        public override void InstallBindings()
        {
            BindStateMachine();
            BindProviders();
            BindSaveLoaders();

            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindSaveLoaders()
        {
            Container.Bind<ISaveLoader>().To<SaveLoader>().AsSingle();
            Container.Bind<IProgressDataSaveLoader>().To<ProgressDataSaveLoader>().AsSingle().WithArguments(_loadData);
        }

        private void BindProviders()
        {
            Container.Bind<IStaticDataProvider>().To<StaticDataProvider>().AsSingle();
            Container.Bind<IProgressDataProvider>().To<ProgressDataProvider>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}