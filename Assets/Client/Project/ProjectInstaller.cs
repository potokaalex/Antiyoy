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
      context.Register(new InputController());
      context.Register(_introView);
      context.Register(_menuView);
      context.Register(_mainMenuView);
      context.Register(new ProjectController());
    }
  }
}