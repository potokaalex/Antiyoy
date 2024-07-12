using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IStaticDataProvider>().To<StaticDataProvider>().AsSingle();
            Container.Bind<IUpdater>().To<Updater>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}