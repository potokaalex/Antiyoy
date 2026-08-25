using Client.Infrastructure;
using Client.Menu;
using Client.Menu.Intro;
using Client.Menu.MainMenu;
using UnityEngine;

namespace Client.Project
{
  public class ProjectInstaller : MonoInstaller, ICoroutineRunner
  {
    [SerializeField] private IntroView _introView;
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private MenuView _menuView;

    public override void Install(Context context)
    {
      context.Register(this, typeof(ICoroutineRunner));
      context.Register(new InputController(), typeof(InputController), typeof(IInitializable), typeof(ITickable));
      context.Register(_introView, typeof(IntroView));
      context.Register(_menuView, typeof(MenuView));
      context.Register(_mainMenuView, typeof(MainMenuView));
      context.Register(new ProjectController(), typeof(ProjectController), typeof(IInitializable));
    }
  }
}