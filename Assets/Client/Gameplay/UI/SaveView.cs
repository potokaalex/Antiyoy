using Client.Menu.MainMenu.SaveLoad;
using Client.Project.Infrastructure;

namespace Client.Gameplay.UI
{
  public class SaveView : SaveLoadView
  {
    private GameplayUI _gameplayUI;
    private SaveLoadSlotView _emptyView;

    protected override void Awake()
    {
      base.Awake();
      _gameplayUI = Locator.Get<GameplayUI>();
    }

    protected override void CreateViews()
    {
      _emptyView = CreateView(null);
      _emptyView.Header = "empty";
      _emptyView.OnClick += OnEmptyViewClick;
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

    private void OnEmptyViewClick(SaveLoadSlotView _)
    {
      if (_emptyView.Map == null)
        _emptyView.SetMap(MapsService.SaveCurrentMapInNewSlot(MapsService.StartMap.Name));
      else
        _emptyView.SetMap(MapsService.SaveCurrentMapInSlot(_emptyView.Map, MapsService.StartMap.Name));
    }

    protected override void OnViewClick(SaveLoadSlotView slotView)
    {
      if (slotView != AutosaveSlotView && slotView != _emptyView) 
        slotView.SetMap(MapsService.SaveCurrentMapInSlot(slotView.Map, MapsService.StartMap.Name));
    }

    protected override void OnViewHold(SaveLoadSlotView slotView)
    {
      if(slotView != _emptyView)
        base.OnViewHold(slotView);
    }
  }
}