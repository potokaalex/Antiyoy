using ClientCode.Data.Scene;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.UI;
using ClientCode.UI.Handlers;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuSceneData _sceneData;

        public override void InstallBindings()
        {
            BindUI();
            
            Container.BindInterfacesTo<ProgressRegister>().AsSingle();
            Container.Bind<MainMenuSceneData>().FromInstance(_sceneData).AsSingle();
            //Container.Bind<MainMenuStartup>().fromNewCo.AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle();
            Container.BindInterfacesTo<MainMenuWindowsHandler>().AsSingle();
            Container.BindInterfacesTo<MainMenuButtonsHandler>().AsSingle();
        }
    }
}