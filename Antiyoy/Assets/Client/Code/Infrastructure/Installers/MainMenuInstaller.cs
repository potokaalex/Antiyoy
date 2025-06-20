using ClientCode.Data.Scene;
using ClientCode.Services.Progress.Actors;
using ClientCode.UI.Factory;
using ClientCode.UI.Models;
using ClientCode.UI.Presenters.MainMenu;
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
            Container.Bind<UIFactory>().AsSingle().WithArguments(_sceneData.UIRoot);
            Container.Bind<IWindowsFactory>().To<WindowsFactory>().AsSingle();
            Container.Bind<ButtonsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuModel>().AsSingle();
            Container.BindInterfacesTo<MainMenuWindowsPresenter>().AsSingle();
            Container.BindInterfacesTo<MainMenuButtonsPresenter>().AsSingle();
            Container.Bind<MainMenuMapSelectButtonsPresenter>().AsSingle();
        }
    }
}