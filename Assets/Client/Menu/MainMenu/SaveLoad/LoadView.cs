using Client.Project.Infrastructure;
using Client.Project.Map.Data;

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

    protected override void OnViewClick(MapData map)
    {
      if (map != null)
      {
        if (map == AutosaveMapData)
          UnityEngine.Debug.Log("Click Autosave");
        else
          UnityEngine.Debug.Log($"Click {map.Name}");
      }
    }

    protected override void OnViewHold(MapData map)
    {
      if (map != null)
      {
        if (map == AutosaveMapData)
          UnityEngine.Debug.Log("Hold Autosave");
        else
          UnityEngine.Debug.Log($"Hold {map.Name}");
      }

      base.OnViewHold(map);
    }
  }
}