using Client.Menu.MainMenu.SaveLoad;
using Client.Project.Infrastructure;

namespace Client.Gameplay.UI
{
  public class SaveView : SaveLoadView
  {
    private GameplayUI _gameplayUI;

    protected override void Awake()
    {
      base.Awake();
      _gameplayUI = Locator.Get<GameplayUI>();
    }

    protected override void OnBackClick()
    {
      Hide();
      _gameplayUI.ShowPause(false);
    }
  }
}