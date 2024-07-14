using ClientCode.Data.Scene;
using ClientCode.UI;
using ClientCode.UI.Buttons;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Presenters;
using ClientCode.UI.Windows.Base;
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
            Container.Bind<MainMenuSceneData>().FromInstance(_sceneData).AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<ILoadButtonHandler>().To<MainMenuPresenter>().AsSingle();
            Container.Bind<IWindowsHandler>().To<WindowsHandler>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
        }
    }
}