using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.UI;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorHud : MonoBehaviour
  {
    [SerializeField] private CustomButton _pauseButton;
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    private MapEditorUI _mapEditorUI;

    private void Awake()
    {
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _pauseButton.OnClick += OnPauseClick;
    }

    private void OnDestroy() => _pauseButton.OnClick -= OnPauseClick;

    public void Show()
    {
      _topAndBottomMenuAnimator.SpeedFactor = 1;
      _topAndBottomMenuAnimator.PlayShow();
    }

    public void Hide()
    {
      _topAndBottomMenuAnimator.SpeedFactor = 2;
      _topAndBottomMenuAnimator.PlayHide();
    }

    private void OnPauseClick()
    {
      _mapEditorUI.PlayHide();
      _mapEditorUI.ShowPause();
    }
  }
}