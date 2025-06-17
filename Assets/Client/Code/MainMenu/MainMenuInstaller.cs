using Zenject;

namespace ClientCode.Infrastructure.States.MapEditor.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        public MainMenuWindow Window;
        
        public override void InstallBindings() => Container.BindInterfacesTo<MainMenuWindow>().FromInstance(Window).AsSingle();
    }
}