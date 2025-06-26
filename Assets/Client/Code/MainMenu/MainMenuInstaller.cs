using Zenject;

namespace Client.Code.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        public MainMenuWindow Window;

        public override void InstallBindings() => Container.BindInterfacesTo<MainMenuWindow>().FromInstance(Window).AsSingle();
    }
}