using Client.Project.Infrastructure;

namespace Client.Menu.MainMenu.SaveLoad
{
  public class LoadView : SaveLoadView
  {
    private MainMenuView _mainMenuView;

    protected override void Awake()
    {
      base.Awake();
      _mainMenuView = Locator.Get<MainMenuView>();
    }

    protected override void OnBackClick()
    {
      Hide();
      _mainMenuView.ShowOptions();
    }
  }
}