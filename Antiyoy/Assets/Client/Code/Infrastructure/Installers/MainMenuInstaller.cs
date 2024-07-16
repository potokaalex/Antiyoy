using ClientCode.Data.Scene;
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

            Container.BindInterfacesTo<ProgressActorsRegister>().AsSingle();
            Container.Bind<MainMenuSceneData>().FromInstance(_sceneData).AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle();
            Container.BindInterfacesTo<MainMenuWindowsHandler>().AsSingle();
            Container.BindInterfacesTo<MainMenuButtonsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuModel>().AsSingle();
        }
    }
}