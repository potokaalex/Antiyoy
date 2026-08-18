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

    protected override void Install()
    {
      Register(typeof(ICoroutineRunner), this);
      Register(new InputController());
      Register(_introView);
      Register(_menuView);
      Register(_mainMenuView);
      Register(new ProjectController());
    }

    protected override void Start()
    {
      base.Start();
      DontDestroyOnLoad(this);
    }
  }
}