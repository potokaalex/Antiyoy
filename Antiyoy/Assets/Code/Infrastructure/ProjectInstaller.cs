using Code.Services.SceneLoader;
using Code.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ConfigProvider _configProvider;
        
        public override void InstallBindings()
        {
            BindStateMachine();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            Container.Bind<StateFactory>().AsSingle();
        }
    }
}