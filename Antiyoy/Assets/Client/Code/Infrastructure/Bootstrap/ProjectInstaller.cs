using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Bootstrap
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private StaticDataProvider _staticDataProvider;
        
        public override void InstallBindings()
        {
            BindStateMachine();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<StaticDataProvider>().FromInstance(_staticDataProvider).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}