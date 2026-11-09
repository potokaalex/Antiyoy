using Client.MapEditor.UI.AgreeView;
using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.Region;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorUI : MonoBehaviour, IInitializable
  {
    [SerializeField] private MapEditorUIView _viewPrefab;
    private MapEditorUIView _view;

    public void Initialize() => _view = Locator.Get<MenuView>().Spawn(_viewPrefab);

    public void PlayShow() => _view.PlayShow();

    public void PlayHide() => _view.PlayHide();

    public void ShowPause() => _view.ShowPause();

    public void ShowPlayerColorsView() => _view.ShowPlayerColorsView();

    public void Clear() => _view.Clear();

    public void HideCurrentPanel() => _view.HideCurrentPanel();

    public void ShowRegionSettings(RegionController region) => _view.ShowRegionSettings(region);

    public void ShowAgreeView(MapEditorAgreeViewParameters parameters) => _view.ShowAgreeView(parameters);
  }
}