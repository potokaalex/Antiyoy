using Client.Menu.MainMenu.SaveLoad;
using Client.Project.Infrastructure;
using Client.Project.Project;

namespace Client.Menu.MainMenu.MapEditor
{
  public class MapEditorLoadView : SaveLoadView
  {
    private MainMenuView _mainMenuView;
    private ProjectController _projectController;

    protected override void Awake()
    {
      base.Awake();
      _mainMenuView = Locator.Get<MainMenuView>();
      _projectController = Locator.Get<ProjectController>();
    }

    public override void DeleteSlot(SaveLoadSlotView slotView) => MapsService.DeleteEditorMap(slotView.Map);

    protected override void OnBackClick()
    {
      Hide();
      _mainMenuView.ShowMapEditor();
    }

    protected override void CreateViews()
    {
      for (var i = 0; i < MapsService.EditorMaps.Count; i++)
        CreateView(MapsService.EditorMaps[i]);
    }

    protected override void OnViewClick(SaveLoadSlotView slotView)
    {
      MapsService.SetStartMap(slotView.Map);
      _projectController.LoadMapEditor();
      Hide();
    }
  }
}