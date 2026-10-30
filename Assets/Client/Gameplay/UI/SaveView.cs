using Client.Menu.MainMenu.SaveLoad;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;

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

    protected override void CreateViews()
    {
      var emptyView = CreateView(null);
      emptyView.Header = "empty";
      emptyView.OnClick += OnEmptyViewClick;
      base.CreateViews();
    }

    protected override void ClearView(SaveLoadSlotView view)
    {
      view.OnClick -= OnEmptyViewClick;
      base.ClearView(view);
    }

    protected override void OnBackClick()
    {
      Hide();
      _gameplayUI.ShowPause(false);
    }

    private void OnEmptyViewClick(MapData _)
    {
      UnityEngine.Debug.Log("Click Empty");
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