using ClientCode.UI;
using ClientCode.UI.Buttons;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings() => Container.Bind<ILoadSceneButtonHandler>().To<LoadSceneButtonHandler>().AsSingle();
    }
}