using ClientCode.Data.Scene;
using ClientCode.Services.Logger;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Actors;
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
            BindLog();

            Container.BindInterfacesTo<ProgressActorsRegister>().AsSingle();
            Container.Bind<MainMenuSceneData>().FromInstance(_sceneData).AsSingle();
        }

        private void BindLog()
        {
            Container.BindInterfacesTo<LogHandlersRegister>().AsSingle();
            Container.BindInterfacesTo<LoggerByPopup>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle();
            Container.BindInterfacesTo<MainMenuWindowsHandler>().AsSingle();
            Container.BindInterfacesTo<MainMenuButtonsHandler>().AsSingle();
        }
    }
}