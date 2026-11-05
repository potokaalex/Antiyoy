using Client.Gameplay.UI.TransitionView;
using Client.MapEditor.UI.Panels.Settings;
using Client.Project.Region;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorUIView : MonoBehaviour
  {
    [SerializeField] private GameTransitionView _gameTransitionView;
    [SerializeField] private MapEditorHud _hud;
    [SerializeField] private MapEditorPauseView _pauseView;
    [SerializeField] private MapEditorPlayerColorsView _playerColorsView;
    [SerializeField] private MapEditorRegionSettingsView _regionSettingsView;

    public void PlayShow()
    {
      _hud.Show();
      _gameTransitionView.PlayToyGameTransition();
    }
    
    public void PlayHide()
    {
      _hud.Hide();
      _gameTransitionView.PlayOutGameTransition();
    }

    public void ShowPause() => _pauseView.Show();

    public void ShowPlayerColorsView() => _playerColorsView.Show();

    public void Clear() => _hud.Clear();

    public void HideCurrentPanel() => _hud.HideCurrentPanel();

    public void ShowRegionSettings(RegionController region) => _regionSettingsView.Show(region);
  }
}