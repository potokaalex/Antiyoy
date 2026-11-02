using Client.Gameplay.UI.TransitionView;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorUIView : MonoBehaviour
  {
    [SerializeField] private GameTransitionView _gameTransitionView;
    [SerializeField] private MapEditorHud _hud;
    [SerializeField] private MapEditorPauseView _pauseView;

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

    public void ShowPlayerColorsView() => _hud.ShowPlayerColorsView();
  }
}