using ClientCode.Data.Static;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.CompositionRoot
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ProjectLoadData _loadData;

        public override void InstallBindings()
        {
            BindStateMachine();
            BindProviders();

            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindProviders()
        {
            Container.Bind<IStaticDataProvider>().To<StaticDataProvider>().AsSingle().WithArguments(_loadData);
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}