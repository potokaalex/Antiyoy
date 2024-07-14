using ClientCode.Data.Scene;
using ClientCode.UI;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map;
using ClientCode.UI.Handlers;
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
            Container.Bind<UIFactory>().AsSingle();
            Container.Bind<IWindowsHandler>().To<MainMenuWindowsHandler>().AsSingle();
            Container.Bind<ILoadButtonHandler>().To<MainMenuButtonsHandler>().AsCached();
            Container.Bind<ISelectMapButtonHandler>().To<MainMenuButtonsHandler>().AsCached();
        }
    }
}