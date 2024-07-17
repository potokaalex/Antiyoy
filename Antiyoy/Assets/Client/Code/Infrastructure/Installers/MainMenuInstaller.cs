using ClientCode.Data.Progress.Map;
using ClientCode.Data.Progress.Project;
using ClientCode.Data.Scene;
using ClientCode.Infrastructure.States.MainMenu;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.StateMachine;
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
            BindProgress();

            Container.Bind<MainMenuSceneData>().FromInstance(_sceneData).AsSingle();
            Container.BindInterfacesTo<StateStartuper<MainMenuEnterState>>().AsSingle();
        }

        private void BindProgress()
        {
            Container.BindInterfacesTo<ProgressActorsRegister<ProjectProgressData>>().AsSingle();
            Container.BindInterfacesTo<ProgressActorsRegister<MapProgressData>>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<UIFactory>().AsSingle().WithArguments(_sceneData.UIRoot);
            Container.Bind<WindowsFactory>().AsSingle();
            Container.Bind<ButtonsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuModel>().AsSingle();
            Container.BindInterfacesTo<MainMenuWindowsPresenter>().AsSingle();
            Container.BindInterfacesTo<MainMenuButtonsPresenter>().AsSingle();
            Container.Bind<MainMenuMapSelectButtonsPresenter>().AsSingle();
        }
    }
}