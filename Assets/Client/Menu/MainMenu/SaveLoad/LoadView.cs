using Client.Project.Infrastructure;
using Client.Project.Project;

namespace Client.Menu.MainMenu.SaveLoad
{
  public class LoadView : SaveLoadView
  {
    private MainMenuView _mainMenuView;
    private ProjectController _projectController;

    protected override void Awake()
    {
      base.Awake();
      _mainMenuView = Locator.Get<MainMenuView>();
      _projectController = Locator.Get<ProjectController>();
    }

    protected override void OnBackClick()
    {
      Hide();
      _mainMenuView.ShowOptions();
    }

    protected override void OnViewClick(SaveLoadSlotView slotView)
    {
      MapsService.SetStartMap(slotView.Map);
      _projectController.LoadGameplay();
      Hide();
    }
  }
}