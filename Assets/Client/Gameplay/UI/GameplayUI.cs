using Client.Infrastructure;
using Client.Menu;
using Client.Region;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class GameplayUI : MonoBehaviour, IInitializable
  {
    [SerializeField] private GameplayUIView _viewPrefab;
    private GameplayUIView _view;

    public void Initialize() => _view = Locator.Get<MenuView>().Spawn(_viewPrefab);

    public void PlayShow() => _view.PlayShow();

    public void ShowRegionUI(RegionController region) => _view.ShowRegionUI(region);

    public void HideRegionUI() => _view.HideRegionUI();

    public void ViewTurnsCount(int value) => _view.ViewTurnsCount(value);

    public void ShowEndScreen(RegionType winner) => _view.ShowEndScreen(winner);

    public void ClearRegionCreation() => _view.ClearRegionCreation();

    public void ShowPause() => _view.ShowPause();

    public void HidePause() => _view.HidePause();
  }
}